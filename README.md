# Fluxo de Caixa

Uma empresa precisa realizar lan�amentos financeiros e gerir seus pagamentos, recebimentos e encargos.

## Requisitos

- � necess�rio uma grande capacidade de processamento devido ao grande volume de lan�amentos financeiros.
- O fluxo de caixa � composto pelos lan�amentos financeiros processados com sucesso.
- � necess�rio uma consulta do fluxo de caixa do dia e dos pr�ximos 30 dias, j� com os lan�amentos futuros.
- Deseja-se calcular a posi��o de caixa, que basicamente mostra o saldo dos lan�amentos do dia e se � positivo ou negativo em rela��o ao dia anterior.

## Regras de Neg�cio

- N�o � permitido fazer lan�amentos no passado.
- A empresa tem um limite para ficar negativo que n�o pode superar o valor de **R$ 20.000,00** no dia; caso ultrapasse n�o ser� poss�vel fazer novos lan�amentos no dia e qualquer tentativa de lan�amento deve falhar e o motivo deve ser informado.
- Caso o valor da conta esteja negativo sobre o mesmo incidem juros de **0,83%** ao dia.

## Modelo de Neg�cio

### Lan�amento Financeiro

```YAML
tipo_de_lancamento: Tipos de lan�amento (pagamento e recebimento),
descricao: Qualquer descri��o sobre o pagamento,
conta_destino: Conta do destinat�rio,
banco_destino: Banco do destinat�rio,
tipo_de_conta: Se a conta � corrente ou poupan�a,
cpf_cnpj_destino: N�mero formatado do CPF ou CNPJ,
valor_do_lancamento: Valor em reais do lan�amento no formato (R$ 0.000,00),
encargos: Valor em reais do lan�amento no formato (R$ 0.000,00),
data_de_lancamento: Data em que o lan�amento ocorreu no formato (dd-mm-aaa)
```

### Fluxo de Caixa

Fluxo de caixa do dia e dos pr�ximos 30 dias.

```YAML
- data: 31-12-2018,
  entradas:
    - data: Data em que a entrada foi lan�ada no fluxo de caixa no formato (dd-mm-aaaa),
      valor: Valor da entrada em reais no formato (R$ 0.000,00)
  saidas:
     - data: Data em que a sa�da foi lan�ada no fluxo de caixa no formato (dd-mm-aaaa),
       valor: Valor da sa�da em reais no formato (R$ 0.000,00)
  encargos:
     - data: Data em que o encargo foi lan�ada no fluxo de caixa no formato (dd-mm-aaaa),
       valor: Valor do encargo em reais no formato (R$ 0.000,00)
  total: Total de lan�amentos do dia em reais no formato (R$ 0.000,00),
  posicao_do_dia: Comparando com o dia anterior se houve aumento do caixa ou queda em percentual no formato (000,0%)
```

## Casos de Uso

### Contexto do Sistema

![Casos de Uso](docs/FluxoDeCaixa_CasosDeUso.png)

### M�dulo de Lan�amentos

![Lan�amentos](docs/FluxoDeCaixa_CasosDeUso_Lancamentos.png)

### M�dulo de Fluxos

![Consolida��o](docs/FluxoDeCaixa_CasosDeUso_Fluxos.png)

### M�dulo de Pol�ticas

![Pol�ticas](docs/FluxoDeCaixa_CasosDeUso_Politicas.png)

## Implementa��o

![Implementa��o](docs/FluxoDeCaixa_Implementacao.png)

### Message Broker

#### Filas

- pagamentos
- recebimentos
- lancamentos-financeiros-processados

## Implanta��o

![Implanta��o](docs/FluxoDeCaixa_Implantacao.png)

# Ferramentas

[Postman Collection](tools/postman_collection.json)
