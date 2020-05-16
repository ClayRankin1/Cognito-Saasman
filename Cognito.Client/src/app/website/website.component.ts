import { Component, OnInit, ElementRef, Output, EventEmitter } from '@angular/core';
import DataGrid from 'devextreme/ui/data_grid';
import { CommonService } from '../shared/services/common.service';
import { environment } from '../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import notify from 'devextreme/ui/notify';
import { WebsiteUrl } from './website.model';

@Component({
    selector: 'app-website',
    templateUrl: './website.component.html',
    styleUrls: ['./website.component.scss'],
})
export class WebsiteComponent implements OnInit {
    private readonly websitesUrl = environment.activeURL + 'api/websites';

    @Output() open = new EventEmitter();
    popupVisible = false;
    url: string = '';
    name: string = '';
    savedUrls: WebsiteUrl[];
    dataGridInstance: DataGrid;
    text: String;
    taskId: string;
    usePopUpEditor = false;
    isPopupVisible = false;
    constructor(
        private elRef: ElementRef,
        private _commService: CommonService,
        public _http: HttpClient
    ) {
        this.taskId = localStorage.getItem('taskId');
        this._commService.updateListsObs.subscribe((response) => {
            this.taskId = response.taskId.toString();
            this.getWebsites().subscribe(
                (data: any) => {
                    this.savedUrls = data;
                },
                (response) => { },
                () => { }
            );
        });
        this.onCancelClick = this.onCancelClick.bind(this);
        this.nameChanged = this.nameChanged.bind(this);
    }

    ngOnInit() {
        let droparea: HTMLElement = this.elRef.nativeElement.querySelector('div.droparea');
        droparea.style.display = 'none';
    }

    saveGridInstance(e) {
        this.dataGridInstance = e.component;
    }

    ngAfterViewInit() {
        var Object = this;
        let droparea: HTMLElement = this.elRef.nativeElement.querySelector('div.droparea');
        let websitearea: HTMLElement = this.elRef.nativeElement.querySelector('div.websiteDiv');

        ['dragenter', 'dragover'].forEach(function (item) {
            droparea.addEventListener(
                item,
                function (e) {
                    droparea.style.display = 'block';
                    e.preventDefault();
                },
                false
            );

            websitearea.addEventListener(
                item,
                function (e) {
                    droparea.style.display = 'block';
                    e.preventDefault();
                },
                false
            );
        });
        // If you are a new user, show the drop zone initially.
        if (!this.savedUrls) {
        } else {
            this.savedUrls.forEach(function (item) {
                if (droparea.style.display === 'none') {
                    websitearea.addEventListener(
                        'dragover',
                        function (e) {
                            e.preventDefault();
                            droparea.style.display = 'block';
                        },
                        false
                    );
                }
            });
        }

        droparea.addEventListener(
            'dragover',
            function (e) {
                e.preventDefault();
            },
            false
        );

        droparea.addEventListener(
            'drop',
            function (e) {
                e.preventDefault();
                Object.url = e.dataTransfer.getData('url');
                Object.popupVisible = true;
            },
            false
        );
    }

    saveWebsite = () => {
        return this._http.post(this.websitesUrl, {
            taskId: this.taskId,
            name: this.name,
            url: this.url,
        });
    };

    getWebsites = () => {
        return this._http.get(`${environment.activeURL}api/websites`, {
            params: { taskId: this.taskId },
        });
    };

    onInitNewRow(e) {
        this.dataGridInstance.option('editing.popup.title', 'Add Website');
    }

    clickItem = (target) => {
        this.open.emit(target.target.name);
    };

    onEditingStart() {
        this.dataGridInstance.option('editing.popup.title', 'Edit Website');
    }

    saveNewRow(e) {
        this.name = e.key.name;
        this.url = e.key.url;
        this.saveWebsite().subscribe();
    }

    urlsend(url) {
        this.popupVisible = false;
    }

    namesend() {
        let that = this;
        this.saveWebsite().subscribe(() => {
            var type = 'success';
            notify('Website Saved!', type, 2000);
            setTimeout(function () {
                that.getWebsites().subscribe(
                    (data: any) => {
                        that.savedUrls = data;
                        that.dataGridInstance.refresh();
                        that.name = '';
                        that.url = '';
                        let droparea: HTMLElement = that.elRef.nativeElement.querySelector(
                            'div.droparea'
                        );
                        droparea.style.display = 'none';
                    },
                    (response) => { },
                    () => { }
                );
            }, 1000);
        });
        this.popupVisible = false;
    }

    deleteRow(e) {
        this._http.delete(`${this.websitesUrl}/${e.data.id}`).subscribe();
    }

    updateRow(e) {
        const headers = new HttpHeaders().set('Content-Type', 'application/json');

        this._http
            .put(
                `${this.websitesUrl}/${e.key.id}`,
                {
                    taskId: e.key.taskId,
                    url: e.newData.url,
                    name: e.newData.name,
                },
                { headers }
            )
            .subscribe(
                (val) => {
                    console.log('PUT call successful value returned in body', val);
                },
                (response) => {
                    console.log('PUT call in error', response);
                },
                () => {
                    console.log('The PUT observable is now completed.');
                }
            );
    }
    usePopUp() {
        this.usePopUpEditor = true;
    }

    onCancelClick = (e) => {
        this.popupVisible = false;
    };

    nameChanged = (e) => {
        this.name = e.value;
    };
}
