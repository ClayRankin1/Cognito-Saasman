import { Injectable } from '@angular/core';

export class User {
    userID: number;
    username: string;
}

export class Project {
    matterID: number;
    name: string;
    fullname: string;
}

let users: User[] = [
    {
        userID: 17212,
        username: 'John',
    },
    {
        userID: 17213,
        username: 'Jeffrey',
    },
    {
        userID: 17214,
        username: 'Raul',
    },
];

let projects: Project[] = [
    {
        matterID: 1,
        name: 'CPX HELP',
        fullname: 'CPX HELP',
    },
    {
        matterID: 2,
        name: 'Misc',
        fullname: 'Misc',
    },
    {
        matterID: 3,
        name: 'TSTM',
        fullname: 'TESTER MATTER1',
    },
    {
        matterID: 4,
        name: 'TSTM2',
        fullname: 'TESTER MATTER2',
    },
    {
        matterID: 5,
        name: 'TWNMN',
        fullname: 'TESTWITH_NO_MATTERNO',
    },
];

@Injectable()
export class Service {
    getUsers() {
        return users;
    }
    getProjects() {
        return projects;
    }
}
