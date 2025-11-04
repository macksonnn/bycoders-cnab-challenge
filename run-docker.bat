@echo off
echo ==========================================
echo CNAB - Sistema de Importacao de Transacoes
echo ==========================================
echo.
echo Iniciando containers Docker...
echo.

docker-compose up --build

echo.
echo Containers encerrados.
pause

