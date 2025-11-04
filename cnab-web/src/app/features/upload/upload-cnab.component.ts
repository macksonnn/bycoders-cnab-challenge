import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { CnabApiService } from '../../core/services/cnab-api.service';

@Component({
    selector: 'app-upload-cnab',
    standalone: true,
    imports: [CommonModule],
    templateUrl: './upload-cnab.component.html',
    styleUrl: './upload-cnab.component.scss'
})
export class UploadCnabComponent {
    selectedFile: File | null = null;
    isDragging = false;
    isUploading = false;
    message: { type: 'success' | 'error', text: string } | null = null;

    constructor(
        private cnabApiService: CnabApiService,
        private router: Router
    ) { }

    onFileSelected(event: Event): void {
        const input = event.target as HTMLInputElement;
        if (input.files && input.files.length > 0) {
            this.selectedFile = input.files[0];
            this.message = null;
        }
    }

    onDragOver(event: DragEvent): void {
        event.preventDefault();
        event.stopPropagation();
        this.isDragging = true;
    }

    onDragLeave(event: DragEvent): void {
        event.preventDefault();
        event.stopPropagation();
        this.isDragging = false;
    }

    onDrop(event: DragEvent): void {
        event.preventDefault();
        event.stopPropagation();
        this.isDragging = false;

        if (event.dataTransfer?.files && event.dataTransfer.files.length > 0) {
            this.selectedFile = event.dataTransfer.files[0];
            this.message = null;
        }
    }

    uploadFile(): void {
        if (!this.selectedFile) {
            this.message = {
                type: 'error',
                text: 'Por favor, selecione um arquivo.'
            };
            return;
        }

        this.isUploading = true;
        this.message = null;

        this.cnabApiService.uploadCnabFile(this.selectedFile).subscribe({
            next: (response) => {
                this.message = {
                    type: 'success',
                    text: 'Arquivo importado com sucesso!'
                };
                this.isUploading = false;
                this.selectedFile = null;

                // Redirecionar para lista de lojas após 2 segundos
                setTimeout(() => {
                    this.router.navigate(['/stores']);
                }, 2000);
            },
            error: (error) => {
                this.message = {
                    type: 'error',
                    text: error.error?.error || 'Erro ao importar arquivo. Tente novamente.'
                };
                this.isUploading = false;
            }
        });
    }

    clearFile(): void {
        this.selectedFile = null;
        this.message = null;
    }

    formatFileSize(bytes: number): string {
        if (bytes === 0) return '0 Bytes';
        const k = 1024;
        const sizes = ['Bytes', 'KB', 'MB'];
        const i = Math.floor(Math.log(bytes) / Math.log(k));
        return Math.round(bytes / Math.pow(k, i) * 100) / 100 + ' ' + sizes[i];
    }
}

