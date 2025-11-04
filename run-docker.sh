#!/bin/bash

echo "=========================================="
echo "CNAB - Sistema de Importação de Transações"
echo "=========================================="
echo ""
echo "Iniciando containers Docker..."
echo ""

docker-compose up --build

echo ""
echo "Containers encerrados."

