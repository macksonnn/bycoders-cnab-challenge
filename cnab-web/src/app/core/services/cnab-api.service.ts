import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Store } from '../models/store.model';
import { environment } from '../../../environments/environment';

@Injectable({
    providedIn: 'root'
})
export class CnabApiService {
    private apiUrl = environment.apiUrl;

    constructor(private http: HttpClient) { }

    /**
     * Faz upload de arquivo CNAB
     */
    uploadCnabFile(file: File): Observable<any> {
        const formData = new FormData();
        formData.append('file', file);

        return this.http.post(`${this.apiUrl}/api/cnab/import`, formData);
    }

    /**
     * Lista todas as lojas com transações e saldo
     */
    getStores(): Observable<Store[]> {
        return this.http.get<Store[]>(`${this.apiUrl}/api/stores`);
    }
}

