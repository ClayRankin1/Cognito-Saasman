import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { Project } from '../schema/project';

@Injectable({
    providedIn: 'root',
})
export class ProjectService extends ApiService<Project> {
    protected endPoint = 'Projects';
}
