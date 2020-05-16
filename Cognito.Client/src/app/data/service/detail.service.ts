import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { Detail } from '../schema/detail';

@Injectable({
    providedIn: 'root',
})
export class DetailService extends ApiService<Detail> {
    protected endPoint = 'Details';
}
