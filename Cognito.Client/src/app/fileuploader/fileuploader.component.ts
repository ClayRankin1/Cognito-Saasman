import { Component, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment';
import { AuthService } from '../auth/auth.service';
import { CommonService } from '../shared/services/common.service';
import { ITasksGridChanges } from '../shared/services/common.service';

@Component({
    selector: 'app-fileuploader',
    templateUrl: './fileuploader.component.html',
    styleUrls: ['./fileuploader.component.scss'],
})
//TO DO - add addtional parameters
export class FileuploaderComponent implements OnInit {
    value: number[] = [];
    uploadUrl: string;
    uploadHeaders: { Authorization: string; taskId: number };

    constructor(private auth: AuthService, private _commService: CommonService) {
        if (this.auth.currentUser) {
            this.uploadUrl =
                environment.activeURL +
                `api/documents?domain=${this.auth.currentUser.domainId}` +
                `&taskId=` +
                localStorage.getItem('taskId') +
                `&projectId=` +
                localStorage.getItem('projectId');
        }
    }

    onUploaded(e) {
        var taskId = parseInt(localStorage.getItem('taskId')),
            projectId = parseInt(localStorage.getItem('projectId'));
        let changedValues: ITasksGridChanges = {
            projectId: projectId,
            taskId: taskId,
            detailTypeId: null,
        };
        this._commService.updateListFn(changedValues);
    }
    ngOnInit() {
        const currentUser = this.auth.currentUser;
        if (currentUser) {
            this.uploadHeaders = {
                Authorization: `Bearer ${currentUser.accessToken}`,
                taskId: +localStorage.getItem('taskId'),
            };
        }
    }
}
