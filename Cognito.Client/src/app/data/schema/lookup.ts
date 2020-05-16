export interface Lookup {
    id: number;
    label: string;
}

export interface TaskTypeLookup extends Lookup {
    description: string;
    isEvent: boolean;
    archivedOn?: Date;
    domainId?: number;
}

export interface DetailTypeLookup extends Lookup {
    detailTypeSourceTypeId: number;
}

export enum LookupType {
    State,
    Time,
    TaskStatus,
    TaskType,
    DetailType,
    DetailTypeSourceType,
}
