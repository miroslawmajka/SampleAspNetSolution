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

    <script src="Scripts/es6-promise.auto.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        'use strict';

        $(document).ready(function()  {
            PageMethods.set_path(PageMethods.get_path() + '.aspx');

            var output = $('tbody#output');
            var currentClick = 1;
            var currentPromise = Promise.resolve();

            $('input#getCurrentTimeAsync').click(function()  {
                getNextServerTime();
            });

            $('input#getCurrentTimeSync').click(function()  {
                // Best solution for sequencing the promises:
                // https://stackoverflow.com/questions/24586110/resolve-promises-one-after-another-i-e-in-sequence/36672042#36672042
                currentPromise = currentPromise
                    .then(function () {
                        return getNextServerTime()
                    });
            });

            function getNextServerTime() {
                var resultCellId = 'cell-result-' + currentClick;
                var responseTimeCellId = 'cell-response-time-' + currentClick;

                var sequence = '<td>' + currentClick + '</td>';
                var callTime = '<td>' + getCurrentTime() + '</td>';
                var ajaxLoader = '<img src="Images/ajax-loader.gif" alt="waiting" />';
                var result = '<td id="' + resultCellId +'">' + ajaxLoader + '</td>';
                var responseTime = '<td id="' + responseTimeCellId + '">' +ajaxLoader +'</td>';

                output.append('<tr>' + sequence + callTime + result + responseTime +'</tr>');
                currentClick++;

                return callAsyncMethod('Mirek')
                    .then(function(response) {
                        parseResponse(response.result)
                    })
                    .catch(function(error) {
                        parseResponse(error.reason)
                    });

                function parseResponse(response) {
                    $('td#' + resultCellId).html('<strong>' + response + '</strong>');
                    $('td#' + responseTimeCellId).html(getCurrentTime());
                }
            }

            function callAsyncMethod(name) {
                return new Promise(function(resolve, reject) {
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
                var date = new Date();

                return date.getHours() + ':' + date.getMinutes() + ':' + date.getSeconds() + '.' + date.getMilliseconds() + '.';
            }
        });
    </script>
</asp:Content>