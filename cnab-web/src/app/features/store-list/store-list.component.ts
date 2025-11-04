import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { CnabApiService } from '../../core/services/cnab-api.service';
import { Store } from '../../core/models/store.model';
import { Transaction } from '../../core/models/transaction.model';

@Component({
    selector: 'app-store-list',
    standalone: true,
    imports: [CommonModule, RouterLink],
    templateUrl: './store-list.component.html',
    styleUrl: './store-list.component.scss'
})
export class StoreListComponent implements OnInit {
    stores: Store[] = [];
    isLoading = true;
    error: string | null = null;
    expandedStores: Set<number> = new Set();

    constructor(private cnabApiService: CnabApiService) { }

    ngOnInit(): void {
        this.loadStores();
    }

    loadStores(): void {
        this.isLoading = true;
        this.error = null;

        this.cnabApiService.getStores().subscribe({
            next: (stores) => {
                this.stores = stores;
                this.isLoading = false;
            },
            error: (error) => {
                this.error = 'Erro ao carregar lojas. Tente novamente.';
                this.isLoading = false;
                console.error('Erro ao carregar lojas:', error);
            }
        });
    }

    toggleStore(storeId: number): void {
        if (this.expandedStores.has(storeId)) {
            this.expandedStores.delete(storeId);
        } else {
            this.expandedStores.add(storeId);
        }
    }

    isStoreExpanded(storeId: number): boolean {
        return this.expandedStores.has(storeId);
    }

    formatCurrency(value: number): string {
        return new Intl.NumberFormat('pt-BR', {
            style: 'currency',
            currency: 'BRL'
        }).format(value);
    }

    formatDate(dateString: string): string {
        const date = new Date(dateString);
        return new Intl.DateTimeFormat('pt-BR', {
            day: '2-digit',
            month: '2-digit',
            year: 'numeric',
            hour: '2-digit',
            minute: '2-digit'
        }).format(date);
    }

    isRevenue(transaction: Transaction): boolean {
        return transaction.signedValue >= 0;
    }
}

