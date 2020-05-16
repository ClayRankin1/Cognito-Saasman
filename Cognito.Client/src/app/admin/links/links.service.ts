import { Injectable } from '@angular/core';

export class Link {
    LinkID: number;
    LeftID: number;
    LeftType: string;
    LeftName: string;
    LeftInfo: string;
    LinkName: string;
    RightInfo: string;
    RightName: string;
    RightType: string;
    RightID: number;
    StartDate: string;
    EndDate: string;
    IsPrimary: boolean;
    UseOnce: boolean;
    DispOrder: string;
}

let links: Link[] = [
    {
        LinkID: 6,
        LeftID: 2,
        LeftType: 'PER',
        LeftName: '',
        LeftInfo: '',
        LinkName: 'Person->Phone',
        RightInfo: '',
        RightName: '',
        RightType: 'PHO',
        RightID: 6,
        StartDate: '2011/02/26',
        EndDate: '',
        IsPrimary: true,
        UseOnce: false,
        DispOrder: '',
    },
    {
        LinkID: 7,
        LeftID: 2,
        LeftType: 'PER',
        LeftName: '',
        LeftInfo: '',
        LinkName: 'Person->Phone',
        RightInfo: '',
        RightName: '',
        RightType: 'PHO',
        RightID: 7,
        StartDate: '2011/02/26',
        EndDate: '',
        IsPrimary: true,
        UseOnce: true,
        DispOrder: '',
    },
    {
        LinkID: 8,
        LeftID: 2,
        LeftType: 'PER',
        LeftName: '',
        LeftInfo: '',
        LinkName: 'Person->Phone',
        RightInfo: '',
        RightName: '',
        RightType: 'PHO',
        RightID: 7,
        StartDate: '2011/02/26',
        EndDate: '',
        IsPrimary: true,
        UseOnce: true,
        DispOrder: '',
    },
    {
        LinkID: 9,
        LeftID: 2,
        LeftType: 'PER',
        LeftName: '',
        LeftInfo: '',
        LinkName: 'Person->Phone',
        RightInfo: '',
        RightName: '',
        RightType: 'PHO',
        RightID: 7,
        StartDate: '2011/02/26',
        EndDate: '',
        IsPrimary: true,
        UseOnce: true,
        DispOrder: '',
    },
    {
        LinkID: 10,
        LeftID: 2,
        LeftType: 'PER',
        LeftName: '',
        LeftInfo: '',
        LinkName: 'Person->Phone',
        RightInfo: '',
        RightName: '',
        RightType: 'PHO',
        RightID: 7,
        StartDate: '2011/02/26',
        EndDate: '',
        IsPrimary: true,
        UseOnce: true,
        DispOrder: '',
    },
    {
        LinkID: 11,
        LeftID: 2,
        LeftType: 'PER',
        LeftName: '',
        LeftInfo: '',
        LinkName: 'Person->Phone',
        RightInfo: '',
        RightName: '',
        RightType: 'PHO',
        RightID: 7,
        StartDate: '2011/02/26',
        EndDate: '',
        IsPrimary: true,
        UseOnce: true,
        DispOrder: '',
    },
    {
        LinkID: 12,
        LeftID: 2,
        LeftType: 'PER',
        LeftName: '',
        LeftInfo: '',
        LinkName: 'Person->Phone',
        RightInfo: '',
        RightName: '',
        RightType: 'PHO',
        RightID: 7,
        StartDate: '2011/02/26',
        EndDate: '',
        IsPrimary: true,
        UseOnce: true,
        DispOrder: '',
    },
    {
        LinkID: 13,
        LeftID: 2,
        LeftType: 'PER',
        LeftName: '',
        LeftInfo: '',
        LinkName: 'Person->Phone',
        RightInfo: '',
        RightName: '',
        RightType: 'PHO',
        RightID: 7,
        StartDate: '2011/02/26',
        EndDate: '',
        IsPrimary: true,
        UseOnce: true,
        DispOrder: '',
    },
];

@Injectable()
export class Service {
    getLinks() {
        return links;
    }
}
