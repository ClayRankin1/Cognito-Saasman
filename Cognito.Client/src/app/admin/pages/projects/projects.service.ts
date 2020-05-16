import { Injectable } from '@angular/core';

export class Employee {
    MatterID: number;
    FullName: string;
    Name: string;
    MatterNo: string;
    OwnerID: number;
    Type: string;
    IsArchived: boolean;
    DomainID: number;
}

export class State {
    ID: number;
    Name: string;
}

let employees: Employee[] = [
    {
        MatterID: 17212,
        FullName: 'TESTER MATTER',
        Name: 'TSTM',
        MatterNo: '1',
        OwnerID: 2095,
        Type: 'B',
        IsArchived: false,
        DomainID: 12,
    },
    {
        MatterID: 17213,
        FullName: 'TESTER MATTER2',
        Name: 'TSTM2',
        MatterNo: '2',
        OwnerID: 2095,
        Type: 'B',
        IsArchived: false,
        DomainID: 12,
    },
    {
        MatterID: 17217,
        FullName: 'TESTWITH_NO_MATTERNO',
        Name: 'TWNMN',
        MatterNo: '3',
        OwnerID: 2095,
        Type: 'B',
        IsArchived: false,
        DomainID: 12,
    },
];

let states: State[] = [
    {
        ID: 1,
        Name: 'Alabama',
    },
    {
        ID: 2,
        Name: 'Alaska',
    },
    {
        ID: 3,
        Name: 'Arizona',
    },
    {
        ID: 4,
        Name: 'Arkansas',
    },
];

@Injectable()
export class ProjectsService {
    getEmployees() {
        return employees;
    }
    getStates() {
        return states;
    }
}
