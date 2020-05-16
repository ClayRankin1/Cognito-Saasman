import { Component, ViewChild, OnInit, ElementRef, AfterViewInit } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Observable, merge, fromEvent, timer } from 'rxjs';
import { debounce, map } from 'rxjs/operators';
import { WebViewerDataService } from './webviewer.service';
import { CommonService } from '../shared/services/common.service';
import { ITasksGridChanges } from '../shared/services/common.service';
import { AuthService } from '../auth/auth.service';
import notify from 'devextreme/ui/notify';
import { Router } from '@angular/router';

declare const WebViewer: any;

declare let window: any;
let checkDuplicateText = window.checkDuplicateText;
let selectedDocument = window.selectedDocument;

@Component({
    selector: 'web-viewer',
    templateUrl: './webviewer.component.html',
    styleUrls: ['./webviewer.component.css'],
})
export class WebViewerComponent implements OnInit, AfterViewInit {
    // Syntax if using Angular 8+
    // true or false depending on code
    public filename: string;
    constructor(
        private router: Router,
        private _commService: CommonService,
        private _documentsDataService: WebViewerDataService,
        private auth: AuthService
    ) { }

    @ViewChild('viewer', { static: true }) viewer: ElementRef;

    // Syntax if using Angular 7 and below
    //@ViewChild('viewer') viewer: ElementRef;

    wvInstance: any;

    ngOnInit() {
        selectedDocument = localStorage.getItem('selectedDocument');

        this.wvDocumentLoadedHandler = this.wvDocumentLoadedHandler.bind(this);
        this.redirectToHome = this.redirectToHome.bind(this);
        let changedValues: ITasksGridChanges = {
            projectId: parseInt(localStorage.getItem('projectId')),
            taskId: parseInt(localStorage.getItem('taskId')),
            detailTypeId: 4,
        };
        this._commService.updateListFn(changedValues);
        // this.bindKeypressEvent().subscribe(($event: KeyboardEvent) =>
        //   this.onKeyPress($event)
        // );
        //?????????????????this.selectedDocument = localStorage.getItem("selectedDocument");
        // $("#detailsgrid").dxDataGrid("instance").columnOption("ItemTypeID", "filterValues", [40])
    }

    //??????????????????????????????????????????????????????????????????????????????????????????
    // onKeyPress($event: KeyboardEvent) {
    //   if (($event.ctrlKey || $event.metaKey) && $event.keyCode == 67)
    //     console.log("CTRL + C");
    //   if (($event.ctrlKey || $event.metaKey) && $event.keyCode == 86)
    //     console.log("CTRL +  V");
    // }
    // private bindKeypressEvent(): Observable<KeyboardEvent> {
    //   const eventsType$ = [
    //     fromEvent(window, "keypress"),
    //     fromEvent(window, "keydown")
    //   ];
    //   return merge(...eventsType$).pipe(
    //     // We prevent multiple next by wait 10ms before to next value.
    //     debounce(() => timer(10)),
    //     // We map answer to KeyboardEvent, typescript strong typing...
    //     map(state => state as KeyboardEvent)
    //   );
    // }
    wvDocumentLoadedHandler(): void {
        // you can access docViewer object for low-level APIs
        const docViewer = this.wvInstance;
        // const annotManager = this.wvInstance.annotManager;
        // // and access classes defined in the WebViewer iframe
        // const { Annotations } = this.wvInstance;
        // const rectangle = new Annotations.RectangleAnnotation();
        // rectangle.PageNumber = 1;
        // rectangle.X = 100;
        // rectangle.Y = 100;
        // rectangle.Width = 250;
        // rectangle.Height = 250;
        // rectangle.StrokeThickness = 5;
        // rectangle.Author = annotManager.getCurrentUser();
        // annotManager.addAnnotation(rectangle);
        // annotManager.drawAnnotations(rectangle.PageNumber);
        // see https://www.pdftron.com/api/web/WebViewer.html for the full list of low-level APIs
    }

    getFile(selectedDocument) {
        if (selectedDocument !== '') var request = new XMLHttpRequest();
        request.open('GET', selectedDocument, true);
        request.responseType = 'blob';
        request.onload = () => {
            var reader = new FileReader();
            reader.readAsDataURL(request.response);
            reader.onload = () => {
                console.log('DataURL:', reader.result as string);
                return request.response;
            };
        };
        request.send();
    }

    ngAfterViewInit(): void {
        // console.log( environment.activeURL + "Documents/18/17302/AlexandraSophieRadcliffPhdThesis.pdf")

        // The following code initiates a new instance of WebViewer.
        // selectedDocument =  this.getFile(selectedDocument);
        WebViewer(
            {
                useDownloader: true,
                path: '../../wv-resources/lib',
                initialDoc: selectedDocument, //this.getFile(selectedDocument),//"https://s3.amazonaws.com/memento.static/Drafts/2095/87755/17332/A%20Response%20to%20Michael%20McClymond%E2%80%99s%20%20Theological%20Critique%20of%20Universalism.rtf?AWSAccessKeyId=AKIAVA3SXR6QOXPQXGWO&Expires=1585211710&Signature=7ZilLMxWGKoFnL9G4vhV%2Feua3m0%3D",// "../../assets/Saasman Admin Critical data.docx", // environment.activeURL + "Documents/18/17302/AlexandraSophieRadcliffPhdThesis.pdf"//'https://pdftron.s3.amazonaws.com/downloads/pl/webviewer-demo.pdf'
                fullAPI: true,
            },
            this.viewer.nativeElement
        ).then((instance) => {
            this.wvInstance = instance;

            // const showSelectedText = () => {
            //   const docViewer = instance.docViewer;
            //   const page = docViewer.getCurrentPage();
            //   const text = docViewer.getSelectedText(page);
            // }

            // now you can access APIs through this.webviewer.getInstance()
            // instance.openElement('notesPanel');
            // see https://www.pdftron.com/documentation/web/guides/ui/apis
            // for the full list of APIs

            // let o = instance.getSelectedText(instance.getCurrentPage());

            // or listen to events from the viewer element
            // this.viewer.nativeElement.addEventListener('pageChanged', (e) => {
            //   const [ pageNumber ] = e.detail;
            //   console.log(`Current page is ${pageNumber}`);
            // });

            // // or from the docViewer instance
            // instance.docViewer.on('textSelected', function(e, quads, text, pageIndex) {
            //   console.log('textSelected ' + text + "pg " + pageIndex);
            // });

            instance.docViewer.on('keyUp', function (e) {
                var keyCode = window.event ? e.which : e.keyCode;
                console.log(keyCode);
            });

            instance.docViewer.on('annotationsLoaded', () => {
                console.log('annotations loaded');
            });

            instance.docViewer.on('documentLoaded', this.wvDocumentLoadedHandler);

            var iframeWindow = instance.iframeWindow;
            iframeWindow.addEventListener('copy', function (e) {
                const docViewer = instance.docViewer;
                const page = docViewer.getCurrentPage();
                const text = docViewer.getSelectedText(page);

                // console.log('Ctrl+C (copy) pressed');
                //prevent double insertions
                const docName = localStorage.getItem('selectedDocumentName');
                if (checkDuplicateText !== text) {
                    InsertExtractionToDetails(text, 'Document: ' + docName + ' - Page: ' + page);
                    checkDuplicateText = text;
                } else {
                }
            });

            const InsertExtractionToDetails = (text, source) => {
                const currentUser = this.auth.currentUser;
                var taskId = parseInt(localStorage.getItem('taskId')),
                    projectId = parseInt(localStorage.getItem('projectId')),
                    domainID = currentUser.domainId,
                    ownerId = currentUser.userId;
                //get globals here for taskId etc
                this._documentsDataService
                    .insertExtractionToDetails(text, taskId, projectId, source, ownerId, domainID)
                    .subscribe(
                        (data) => {
                            let i = data;
                            //update Details?????????????

                            let changedValues: ITasksGridChanges = {
                                projectId: projectId,
                                taskId: taskId,
                                detailTypeId: 4,
                            };
                            this._commService.updateListFn(changedValues);
                            var type = 'success';
                            notify('Record added to details', type, 1000);
                        },
                        (response) => { },
                        () => { }
                    );
            };
        });
    }

    redirectToHome() {
        this.router.navigate(['/home']);
    }
}
