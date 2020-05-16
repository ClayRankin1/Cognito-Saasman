import { Component, enableProdMode, OnInit } from '@angular/core';
import { Service, User, Project } from './projectusers.service';
import { confirm } from 'devextreme/ui/dialog';
import DataGrid from 'devextreme/ui/data_grid';

if (!/localhost/.test(document.location.host)) {
    enableProdMode();
}

@Component({
    selector: 'app-projectusers',
    templateUrl: './projectusers.component.html',
    styleUrls: ['./projectusers.component.scss'],
    providers: [Service],
})
export class ProjectusersComponent implements OnInit {
    users: User[];
    projects: Project[];
    selectedUsersKeys: number[] = [];
    userGridInstance: DataGrid;

    constructor(service: Service) {
        this.users = service.getUsers();
        this.projects = service.getProjects();
    }

    ngOnInit() {}

    getUserGridInstance(e) {
        this.userGridInstance = e.component;
    }

    userscontentReady(e) {}

    projectscontentReady(e) {}

    selectionChanged(e) {
        this.selectedUsersKeys = e.selectedRowKeys;
    }

    projectclear() {}

    link() {}

    userclear() {}
}
