#language: pt-br
Funcionalidade: Fluxo de Caixa
	Para operar com dinheiro em caixa
	Enquanto **empresa**
	Eu quero *gerenciar* meus **pagamentos** e **recebimentos**

Contexto: Estou na minha conta do Nubank
	Dado que eu tenho uma conta 'Nubank' (banco: '001', conta Corrente: '567', cpf: '096')

Cenário: Empresa lança um pagamento com sucesso
	Dado que a minha conta está vazia
	Quando lanço um pagamento de 'Conta de luz' de R$ 50,00 hoje na minha conta
	Então ao consultar o fluxo de caixa de hoje o total deve ser de R$ 73,45
	E uma saída 'Conta de luz' no valor de R$ 50,00 deve existir no fluxo
