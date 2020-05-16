import { Injectable } from '@angular/core';

export class Contact {
    ContactID: number;
    FirstName: string;
    LastName: string;
    Name: string;
    FormalName: string;
    PrTitle: string;
    PrEntity: string;
    PrAddress: string;
    PrCity: string;
    PrRegion: string;
    PrPostalcode: string;
    PrCountry: string;
    PrPhone: string;
    PrEmail: string;
    Notes: string;
    KeyName: string;
    Added: string;
}

let contacts: Contact[] = [
    {
        ContactID: 26042,
        FirstName: 'Jeffrey',
        LastName: 'Corman',
        Name: 'Jeffrey',
        FormalName: 'Jeffrey Corman',
        PrTitle: 'test',
        PrEntity: '',
        PrAddress: '',
        PrCity: '',
        PrRegion: '',
        PrPostalcode: '',
        PrCountry: '',
        PrPhone: '',
        PrEmail: '',
        Notes: '',
        KeyName: '',
        Added: '2011/02/24',
    },
    {
        ContactID: 260423,
        FirstName: 'Jeffrey',
        LastName: 'Corman',
        Name: 'Jeffrey',
        FormalName: 'Jeffrey Corman',
        PrTitle: 'test',
        PrEntity: '',
        PrAddress: '',
        PrCity: '',
        PrRegion: '',
        PrPostalcode: '',
        PrCountry: '',
        PrPhone: '',
        PrEmail: '',
        Notes: '',
        KeyName: '',
        Added: '2011/02/24',
    },
    {
        ContactID: 26044,
        FirstName: 'Jeffrey',
        LastName: 'Corman',
        Name: 'Jeffrey',
        FormalName: 'Jeffrey Corman',
        PrTitle: 'test',
        PrEntity: '',
        PrAddress: '',
        PrCity: '',
        PrRegion: '',
        PrPostalcode: '',
        PrCountry: '',
        PrPhone: '',
        PrEmail: '',
        Notes: '',
        KeyName: '',
        Added: '2011/02/24',
    },
    {
        ContactID: 26045,
        FirstName: 'Jeffrey',
        LastName: 'Corman',
        Name: 'Jeffrey',
        FormalName: 'Jeffrey Corman',
        PrTitle: 'test',
        PrEntity: '',
        PrAddress: '',
        PrCity: '',
        PrRegion: '',
        PrPostalcode: '',
        PrCountry: '',
        PrPhone: '',
        PrEmail: '',
        Notes: '',
        KeyName: '',
        Added: '2011/02/24',
    },
    {
        ContactID: 26046,
        FirstName: 'Jeffrey',
        LastName: 'Corman',
        Name: 'Jeffrey',
        FormalName: 'Jeffrey Corman',
        PrTitle: 'test',
        PrEntity: '',
        PrAddress: '',
        PrCity: '',
        PrRegion: '',
        PrPostalcode: '',
        PrCountry: '',
        PrPhone: '',
        PrEmail: '',
        Notes: '',
        KeyName: '',
        Added: '2011/02/24',
    },
];

@Injectable()
export class Service {
    getContacts() {
        return contacts;
    }
}
