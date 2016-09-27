<%@ Page Title="JS Async Test" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" 
    CodeBehind="JsAsyncTest.aspx.cs" Inherits="SampleAspNetWebFormsWebProject.JsAsyncTest" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %></h2>

    <p>
        <input type="button" id="getCurrentTimeAsync" value="Get Current Time (Async)" />
        <input type="button" id="getCurrentTimeSync" value="Get Current Time (Sync)" />
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

            const output = $('tbody#output');
            let currentClick = 1;
            let currentPromise = Promise.resolve();

            $('input#getCurrentTimeAsync').click(() => {
                getNextServerTime();
            });

            $('input#getCurrentTimeSync').click(() => {
                // Best solution for sequencing the promises:
                // https://stackoverflow.com/questions/24586110/resolve-promises-one-after-another-i-e-in-sequence/36672042#36672042
                currentPromise = currentPromise
                    .then(() => getNextServerTime());
            });

            function getNextServerTime() {
                const resultCellId = `cell-result-${currentClick}`;
                const responseTimeCellId = `cell-response-time-${currentClick}`;

                const sequence = `<td>${currentClick}</td>`;
                const callTime = `<td>${getCurrentTime()}</td>`
                const ajaxLoader = '<img src="Images/ajax-loader.gif" alt="waiting" />';
                const result = `<td id="${resultCellId}">${ajaxLoader}</td>`;
                const responseTime = `<td id="${responseTimeCellId}">${ajaxLoader}</td>`;

                output.append(`<tr>${sequence}${callTime}${result}${responseTime}</tr>`);
                currentClick++;

                return callAsyncMethod('Mirek')
                    .then(response => parseResponse(response.result))
                    .catch(error => parseResponse(error.reason));

                function parseResponse(response) {
                    $(`td#${resultCellId}`).html(`<strong>${response}</strong>`);
                    $(`td#${responseTimeCellId}`).html(getCurrentTime());
                }
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

            function getCurrentTime() {
                let date = new Date();

                return `${date.getHours()}:${date.getMinutes()}:${date.getSeconds()}.${date.getMilliseconds()}.`;
            }
        });
    </script>
</asp:Content>