import { Injectable } from '@angular/core';

export class User {
    UserID: number;
    Disabled: boolean;
    Email: string;
    UserName: string;
    SecurityID: number;
    DomainID: number;
    ContactID: number;
}

export class Security {
    SecurityID: number;
    Name: string;
}

export class Contact {
    ContactID: number;
    Name: string;
}

let users: User[] = [
    {
        UserID: 6,
        Disabled: true,
        Email: 'monetscatapp@gmail.com',
        UserName: 'MC',
        SecurityID: 1,
        DomainID: 12,
        ContactID: 2,
    },
    {
        UserID: 7,
        Disabled: false,
        Email: 'TEST@TEST.com',
        UserName: 'TESTer',
        SecurityID: 2,
        DomainID: 12,
        ContactID: 3,
    },
];

let securities: Security[] = [
    {
        SecurityID: 1,
        Name: 'Domain Owner',
    },
    {
        SecurityID: 2,
        Name: 'Monitor',
    },
    {
        SecurityID: 3,
        Name: 'Worker',
    },
    {
        SecurityID: 4,
        Name: 'Matter Proxy',
    },
];

let contacts: Contact[] = [
    {
        ContactID: 1,
        Name: 'Monet Cat',
    },
    {
        ContactID: 2,
        Name: 'Horace Fair',
    },
    {
        ContactID: 3,
        Name: 'Wayne Fair',
    },
    {
        ContactID: 4,
        Name: 'Jeffrey Corman',
    },
];

@Injectable()
export class Service {
    getUsers() {
        return users;
    }
    getSecurities() {
        return securities;
    }
    getContacts() {
        return contacts;
    }
}
