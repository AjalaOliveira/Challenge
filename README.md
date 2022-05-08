# Challenge.API

API desenvolvida em .NET 5.0 com testes automatizados.

### Introdução
API desenvolvida para receber uma lista de pessoas que pretendem ganhar uma casa popular. Como resultado deverá devolver a lista recebida ordenada conforme os critérios abaixo:

- Renda total da família até 900 reais - valendo 5 pontos;
- Renda total da família de 901 à 1500 reais - valendo 3 pontos;
- Famílias com 3 ou mais dependentes  (lembrando que dependentes maiores de 18 anos não contam) - valendo 3 pontos;
- Famílias com 1 ou 2 dependentes  (lembrando que dependentes maiores de 18 anos não contam) - valendo 2 pontos.

### Regra de validação
Uma família sempre possuirá um único pretendente e um único cônjuge.

### Resultado
O resultado esperado é que as famílias, na listagem, estejam pontuadas de acordo com os critérios que foram atendidos (cada família pode pontuar uma única vez por critério, além de poder atender todos os critérios ou nenhum deles) e ordenadas pela pontuação, favorecendo as famílias melhores pontuadas.
