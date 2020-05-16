import { Component, ElementRef, OnDestroy, AfterViewInit, OnInit } from '@angular/core';
import {
    create,
    createOptions,
    RichEdit,
    RibbonTab,
    RibbonButtonItem,
    RibbonTabType,
} from 'devexpress-richedit';
import { environment } from 'src/environments/environment';
import { AuthService } from '../auth/auth.service';
import { User } from '../auth/user.model';
import { ActivatedRoute, Router } from '@angular/router';
import { confirm } from 'devextreme/ui/dialog';
import { HttpClient, HttpParams } from '@angular/common/http';
import notify from 'devextreme/ui/notify';

@Component({
    selector: 'app-texteditor',
    templateUrl: './texteditor.component.html',
    styleUrls: ['./texteditor.component.scss'],
})
export class TexteditorComponent implements AfterViewInit, OnDestroy {
    private rich: RichEdit;
    public popupVisible = false;
    public filename = '';
    private currentUser: User;
    public docstatus: string;
    public dropPopUpVisible = false;
    public fileId: number;
    constructor(
        private element: ElementRef,
        private auth: AuthService,
        private route: ActivatedRoute,
        public _http: HttpClient,
        private router: Router
    ) {
        // see if file alrady exists - routed from documents grid for *.docx
        if (this.route.snapshot.paramMap.get('id') !== null) {
            console.log(this.route.snapshot.paramMap.get('id'));
            this.filename = this.route.snapshot.paramMap.get('id');
            this.docstatus = 'draft';
            this.fileId = parseInt(this.route.snapshot.paramMap.get('fileId'));
        }

        this.currentUser = this.auth.currentUser;
    }

    ngAfterViewInit(): void {
        const options = createOptions();

        options.width = '1700px';
        options.height = '800px';
        //options.exportUrl = 'http://localhost:5000/api/documents/Export'
        options.exportUrl = environment.activeURL + 'api/documents/DraftSaveAs';
        // headers: { Authorization: `Bearer ${currentUser.token}` },

        const findTabFile = options.ribbon.getTab(RibbonTabType.File);
        //  const newFindTab =  options.ribbon.insertTab(new RibbonTab('FindXXXXX', 1), 2);

        //IF loaded boolean is true - then disable "save as"
        // if false the enable "save as" - and disable "save"
        //need field?? - draft - completed???

        // when completed convert fromm docx to pdf

        // LOAD document

        // TO SET BUCKET - CONSTRUCT **FILENAME** USING DOMAIN AND ACTID - THEN SPLIT OUT FILE NAME WITHIN API!!!!!!
        //  var key = $"{domain}/{userFromRepo.Id}/{Path.GetRandomFileName().Replace(".", "")}{Path.GetExtension(fileName)}";
        const SaveAs = 'SaveAs';
        findTabFile.insertItem(new RibbonButtonItem(SaveAs, 'Save As', { beginGroup: true }));
        // options.events.customCommandExecuted = (s, e) => {
        //   if (e.commandName === SaveAs) {
        //
        //     this.popupVisible=true;

        //    //   const selectedText = s.selection.activeSubDocument.getText(selectedInterval);
        //      // window.open(`https://www.google.com/search?q=${selectedText}`, '_blank');

        //   }
        //};

        const ConvertStatusToCompleted = 'ConvertStatusToCompleted';
        findTabFile.insertItem(
            new RibbonButtonItem(ConvertStatusToCompleted, 'Convert', { beginGroup: false })
        );

        options.events.saved = () => {
            var type = 'success';
            notify('Document Saved!', type, 2000);
        };

        options.events.customCommandExecuted = (s, e) => {
            if (e.commandName === ConvertStatusToCompleted) {
                //prompt - confirm "Are you sure???"
                //set status to "Completed"
                //Save
                //redirect to extractor??
                let result = confirm(
                    "Are you sure you want to convert status to 'Completed' (you will no longer be able to edit this document)?",
                    'Confirm'
                );
                result.then((dialogResult) => {
                    if (dialogResult) {
                        this.docstatus = 'Completed';
                        this.converStatus(this.fileId).subscribe(() => {
                            localStorage.setItem('selectedDocument', this.filename);
                            localStorage.setItem('selectedDocumentName', this.filename);
                            this.router.navigate(['/documentextractor']);
                        });
                    }
                });
            }
            if (e.commandName === SaveAs) {
                this.popupVisible = true;

                //   const selectedText = s.selection.activeSubDocument.getText(selectedInterval);
                // window.open(`https://www.google.com/search?q=${selectedText}`, '_blank');
            }
        };

        //      options.events.saving = (s, e) => {
        //notify("Record added to details", type, 1000);
        //     //   this.popupVisible = false;
        //     //   s.fileName =  this.filename;
        //       s.documentSaveFormat = 2;
        //     if(this.docstatus == "Completed")
        //      e.reason="ConvertToCompleted";
        //
        //     //   // console.log(s.fileName);
        //     //   // console.log(s.documentSaveFormat);
        //      }

        this.rich = create(this.element.nativeElement.firstElementChild, options);

        //  this.rich.openDocument(this.filename);

        if (this.filename !== '' && this.filename !== undefined) {
            //const fileNameShort = this.filename;
            var request = new XMLHttpRequest();
            request.open('GET', this.filename, true);
            request.responseType = 'blob';
            request.onload = () => {
                var reader = new FileReader();
                reader.readAsDataURL(request.response);
                reader.onload = () => {
                    console.log('DataURL:', reader.result as string);
                    this.rich.openDocument(reader.result as string, this.filename, 4);
                };
            };
            request.send();
        }
    }

    ngOnDestroy() {
        if (this.rich) {
            this.rich.dispose();
            this.rich = null;
        }
    }

    saveAsState(e) {
        if (!this.rich.fileName.includes('.docx'))
            this.rich.fileName = this.rich.fileName + '.docx';
        this.rich.documentSaveFormat = 4;
        let fn = this.rich.fileName;

        // $"{domain}/{userFromRepo.Id}/{Path.GetRandomFileName().Replace(".", "")}{Path.GetExtension(fileName)}";
        this.rich.fileName =
            'Drafts/' +
            this.currentUser.userId +
            '/' +
            localStorage.getItem('actId') +
            '/' +
            localStorage.getItem('matterId') +
            '/' +
            fn +
            '/' +
            this.currentUser.domainId;

        this.rich.saveDocument();
        this.popupVisible = false;
    }

    valueChanged(data) {
        this.rich.fileName = data.value;
    }
    myFunction(event) {
        event.stopPropagation();
        this.dropPopUpVisible = true;
    }
    txtDropped(e) {
        this.dropPopUpVisible = false;
    }

    converStatus(fileId: number) {
        return this._http.get(environment.activeURL + 'api/documents/ConvertStatus/?id=' + fileId);
    }
}
