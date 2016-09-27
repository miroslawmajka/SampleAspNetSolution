<%@ Page Title="JS Async Test" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" 
    CodeBehind="JsAsyncTest.aspx.cs" Inherits="SampleAspNetWebFormsWebProject.JsAsyncTest" %>

<%-- TODO: add a new page just for old IE11 with the old JS syntax and extract the javascript in separatet module --%>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %></h2>

    <p>
        <input type="button" id="getCurrentTimeAsync" value="Get Current Time (Asynchronous)" />
        <input type="button" id="getCurrentTimeSeq" value="Get Current Time (Sequential)" />
        <input type="button" id="clearTable" value="Clear Table" />
    </p>
    
    <table class="js-async-test-table">
        <thead>
            <tr>
                <th>Sequence</th>
                <th>Call Time</th>
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

            $('input#getCurrentTimeAsync').click(() => {
                const cellIds = addNewRow();

                callAsyncMethod(SAMPLE_USER_NAME)
                    .then(response => parseResponse(cellIds, response.result))
                    .catch(error => parseResponse(cellIds, error.reason));
            });

            $('input#getCurrentTimeSeq').click(() => {
                const cellIds = addNewRow();

                // Best solution for sequencing the promises:
                // https://stackoverflow.com/questions/24586110/resolve-promises-one-after-another-i-e-in-sequence/36672042#36672042
                currentPromise = currentPromise
                    .then(() => {
                        return callAsyncMethod(SAMPLE_USER_NAME)
                            .then(response => parseResponse(cellIds, response.result))
                            .catch(error => parseResponse(cellIds, error.reason));
                    });
            });

            $('input#clearTable').click(() => {
                output.empty();

                // TODO: break the cycle when table cleared (stop existing calls)
            });

            function addNewRow() {
                const resultCellId = `cell-result-${seqNumber}`;
                const responseTimeCellId = `cell-response-time-${seqNumber}`;

                const sequence = `<td>${seqNumber}</td>`;
                const callTime = `<td>${getCurrentTime()}</td>`
                const ajaxLoader = '<img src="Images/ajax-loader.gif" alt="waiting" />';
                const result = `<td id="${resultCellId}">${ajaxLoader}</td>`;
                const responseTime = `<td id="${responseTimeCellId}">${ajaxLoader}</td>`;

                output.append(`<tr>${sequence}${callTime}${result}${responseTime}</tr>`);

                seqNumber++;

                return {
                    resultCellId: resultCellId,
                    responseTimeCellId: responseTimeCellId
                };
            }

            function callAsyncMethod(name) {
                return new Promise((resolve, reject) => {
                    PageMethods.GetCurrentTime(name, OnSuccess, OnError);

                    function OnSuccess(result, userContext, methodName) {
                        return resolve({
                            result: result,
                            userContext: userContext,
                            methodName: methodName
                        });
                    }

                    function OnError(error, userContext, methodName) {
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
                let date = new Date();

                // TODO: add padding for milliseconds

                return `${date.getHours()}:${date.getMinutes()}:${date.getSeconds()}.${date.getMilliseconds()}`;
            }
        });
    </script>
</asp:Content>