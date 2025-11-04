import { Routes } from '@angular/router';

export const routes: Routes = [
    {
        path: '',
        redirectTo: '/upload',
        pathMatch: 'full'
    },
    {
        path: 'upload',
        loadComponent: () => import('./features/upload/upload-cnab.component').then(m => m.UploadCnabComponent)
    },
    {
        path: 'stores',
        loadComponent: () => import('./features/store-list/store-list.component').then(m => m.StoreListComponent)
    },
    {
        path: '**',
        redirectTo: '/upload'
    }
];

