import CustomStore from 'devextreme/data/custom_store';

export interface TypeLookup {
    id: number;
    label: string;
}

export interface TimeLookup {
    id: number;
    label: string;
}

export interface StatusLookup {
    store: CustomStore;
}

export interface OwnerLookup {
    id: number;
    fullName: string;
}

export interface TeamLookup {
    id: number;
    fullName: string;
}

export interface TaskRowData {
    accrued: string;
    accruedTotal: number;
    createdByUser: string;
    createdByUserId: number;
    dateAdded: string;
    dateUpdated: string;
    description: string;
    detailsCount: number;
    endDate: string;
    groupDate: string;
    id: number;
    isEvent: boolean;
    nextDate: string;
    nextTime: string;
    ownerId: number;
    projectId: number;
    projectName: string;
    projectTypeId: number;
    status: number;
    subtasks: number[];
    taskTypeId: number;
    timeId: number;
    updatedByUser: string;
    updatedByUserId: number;
}
