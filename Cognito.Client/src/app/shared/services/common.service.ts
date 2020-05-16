import { Injectable } from '@angular/core';
import { Subject, Observable } from 'rxjs';

export interface ITasksGridChanges {
    projectId: number;
    taskId: number;
    detailTypeId: number;
}

@Injectable({
    providedIn: 'root',
})
export class CommonService {
    private updateLists = new Subject<ITasksGridChanges>();
    updateListsObs = this.updateLists.asObservable();

    updateListFn(_changedValues: ITasksGridChanges) {
        this.updateLists.next(_changedValues);
    }
}
