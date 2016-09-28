<%@ Page Title="Java Script Sequence" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" 
    CodeBehind="JsAsyncTest.aspx.cs" Inherits="SampleAspNetWebFormsWebProject.JsAsyncTest" %>

<%-- TODO: add a new page just for old IE11 with the old JS syntax and extract the javascript in separatet module --%>
<asp:Content ID="ctnMainContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %></h2>
    <p>
        Sample JavaScript code for calling a current server time webservice. The first button fires away asynchronously 
        and does not wait for a reply. The second button calls the webservice and chains subsequent calls so that
        the next call can only start when the previous one got a result.
    </p>
    <p>
        <span class="left">
            <input type="button" id="getCurrentTimeAsync" value="Get Current Time (Asynchronous)" class="btn btn-default" />
            <input type="button" id="getCurrentTimeSeq" value="Get Current Time (Sequential)" class="btn btn-default" />
            <input type="button" id="clearTable" value="Clear Table" class="btn btn-default" />
        </span>
        <span class="right">
            Pending Calls:
            <input type="text" id="txtPendingCalls" class="form-control pending-calls-count" maxlength="5" />
        </span>
    </p>

    <div class="float-clear"></div>

    <br />
    
    <table class="js-async-test-table table">
        <thead>
            <tr>
                <th>Sequence</th>
                <th>Call Time</th>
                <th>Sequential</th>
                <th>Result</th>
                <th>Response Time</th>
            </tr>
        </thead>
        <tbody id="output"></tbody>
    </table>

    <script type="text/javascript">
        'use strict';

        $(document).ready(() => {
            PageMethods.set_path(PageMethods.get_path() + '.aspx');

            const SAMPLE_USER_NAME = 'Bob';
            const output = $('tbody#output');

            let seqNumber = 1;
            let currentPromise = Promise.resolve();
            let callsCancelled = false;
            let pendingCalls = 0;

            updatePendingCalls();

            $('input#getCurrentTimeAsync').click(() => {
                uncancelCalls();

                const cellIds = addNewRow(false);
                increasePendingCalls();

                callAsyncMethod(SAMPLE_USER_NAME)
                    .then(response => parseResponse(cellIds, response.result))
                    .catch(error => parseResponse(cellIds, error.reason));
            });

            $('input#getCurrentTimeSeq').click(() => {
                uncancelCalls();

                const cellIds = addNewRow(true);
                increasePendingCalls();

                // Best solution for sequencing the promises:
                // https://stackoverflow.com/questions/24586110/resolve-promises-one-after-another-i-e-in-sequence/36672042#36672042
                currentPromise = currentPromise
                    .then(() => callAsyncMethod(SAMPLE_USER_NAME))
                    .then(response => parseResponse(cellIds, response.result))
                    .catch(error => parseResponse(cellIds, error.reason));
            });

            $('input#clearTable').click(() => {
                cancelCalls();
                output.empty();
            });

            function addNewRow(isSequential) {
                const resultCellId = `cell-result-${seqNumber}`;
                const responseTimeCellId = `cell-response-time-${seqNumber}`;

                const sequence = `<td>${seqNumber}</td>`;
                const callTime = `<td>${getCurrentTime()}</td>`
                const sequential = `<td>${isSequential.toString()}</td>`;
                const ajaxLoader = '<img src="Images/ajax-loader.gif" alt="waiting" />';
                const result = `<td id="${resultCellId}">${ajaxLoader}</td>`;
                const responseTime = `<td id="${responseTimeCellId}">${ajaxLoader}</td>`;

                output.append(`<tr>${sequence}${callTime}${sequential}${result}${responseTime}</tr>`);

                seqNumber++;

                return {
                    resultCellId: resultCellId,
                    responseTimeCellId: responseTimeCellId
                };
            }

            function callAsyncMethod(name) {
                return new Promise((resolve, reject) => {
                    if (callsAreCancelled()) {
                        decreasePendingCalls();

                        return resolve({});
                    }

                    PageMethods._staticInstance.GetCurrentTime(name, OnSuccess, OnError);

                    function OnSuccess(result, userContext, methodName) {
                        decreasePendingCalls();
                        resolve({
                            result: result,
                            userContext: userContext,
                            methodName: methodName
                        });
                    }

                    function OnError(error, userContext, methodName) {
                        decreasePendingCalls();
                        reject({
                            reason: error._message,
                            userContext: userContext,
                            methodName: methodName
                        });
                    }
                });
            }

            function parseResponse(cellIds, response) {
                $(`td#${cellIds.resultCellId}`).html(`<strong>${response}</strong>`);
                $(`td#${cellIds.responseTimeCellId}`).html(getCurrentTime());
            }

            function getCurrentTime() {
                const date = new Date();
                const hours = padNumber(date.getHours(), 2);
                const minutes = padNumber(date.getMinutes(), 2);
                const seconds = padNumber(date.getSeconds(), 2);
                const milliseconds = padNumber(date.getMilliseconds(), 3);

                return `${hours}:${minutes}:${seconds}.${milliseconds}`;
            }

            function padNumber(str, max) {
                str = str.toString();

                return str.length < max ? padNumber(`0${str}`, max) : str;
            }

            function cancelCalls() {
                callsCancelled = true;
            }

            function uncancelCalls() {
                callsCancelled = false;
            }

            function callsAreCancelled() {
                return callsCancelled;
            }

            function increasePendingCalls() {
                pendingCalls++;
                updatePendingCalls();
            }

            function decreasePendingCalls() {
                pendingCalls--;
                updatePendingCalls();
            }

            function updatePendingCalls() {
                $('input#txtPendingCalls').val(pendingCalls.toString());
            }
        });
    </script>
</asp:Content>