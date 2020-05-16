import { Component, ViewEncapsulation } from '@angular/core';

@Component({
    selector: 'app-report-viewer',
    encapsulation: ViewEncapsulation.None,
    templateUrl: './report-viewer.component.html',
    styleUrls: [
        '../../../node_modules/jquery-ui/themes/base/all.css',
        '../../../node_modules/@devexpress/analytics-core/dist/css/dx-analytics.common.css',
        '../../../node_modules/@devexpress/analytics-core/dist/css/dx-analytics.light.css',
        '../../../node_modules/devexpress-reporting/dist/css/dx-webdocumentviewer.css',
    ],
})
export class ReportViewerComponent {
    title = 'DXReportViewerSample';
    reportUrl: string = '';
    hostUrl: string = 'http://localhost:5000/';
    invokeAction: string = '/WebDocumentViewer/Invoke';
}
