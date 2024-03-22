$(document).ready(function () {
    $('#formCadastroBeneficiario').submit(function (e) {
        e.preventDefault();

        const inputNome = $('#formCadastroBeneficiario #Nome');
        const inputCPF = $('#formCadastroBeneficiario #CPF');

        if (!validateCPF(inputCPF[0])) {
            return false;
        }

        const idTemporario = "temp_" + Date.now();
        SetaBeneficiarioLocalStorage(inputNome.val(), inputCPF.val(), idTemporario);

        inputNome.val('');
        inputCPF.val('');

        preencherDadosBeneficiarios();
    });

    $('#formCadastro').submit(function (e) {
        e.preventDefault();

        const inputCPF = $(this).find("#CPF");
        if ($('#CadastroBeneficiario').hasClass('show') || !validateCPF(inputCPF[0])) {
            return;
        }

        let dadosGrid = localStorage.getItem('dadosGrid');
        if (!dadosGrid) {
            dadosGrid = [];
        } else {
            dadosGrid = JSON.parse(dadosGrid);
        }

        $.ajax({
            url: urlPost,
            method: "POST",
            data: {
                "NOME": $(this).find("#Nome").val(),
                "CEP": $(this).find("#CEP").val(),
                "Email": $(this).find("#Email").val(),
                "Sobrenome": $(this).find("#Sobrenome").val(),
                "Nacionalidade": $(this).find("#Nacionalidade").val(),
                "Estado": $(this).find("#Estado").val(),
                "Cidade": $(this).find("#Cidade").val(),
                "Logradouro": $(this).find("#Logradouro").val(),
                "Telefone": $(this).find("#Telefone").val(),
                "CPF": $(this).find("#CPF").val(),
                "Beneficiarios": dadosGrid
            },
            error:
                function (r) {
                    if (r.status == 400 || r.status == 422)
                        ModalDialog("Ocorreu um erro", r.responseJSON);
                    else if (r.status == 500)
                        ModalDialog("Ocorreu um erro", "Ocorreu um erro interno no servidor.");
                },
            success:
                function (r) {
                    ModalDialog("Sucesso!", r)
                    localStorage.removeItem('dadosGrid');
                }
        });
    })

    $('#formAlteraBeneficiario').submit(function (e) {
        e.preventDefault();

        const inputCPF = $(this).find("#CPF");
        if (!validateCPF(inputCPF[0])) {
            return;
        }

        const beneficiarioId = $(this).find("#Id").val();
        const beneficiarios = JSON.parse(localStorage.getItem('dadosGrid'));
        const beneficiarioIndex = beneficiarios.findIndex(function (element) {
            return element.ID == beneficiarioId;
        });

        beneficiarios[beneficiarioIndex].NOME = $(this).find("#Nome").val();
        beneficiarios[beneficiarioIndex].CPF = $(this).find("#CPF").val();
        localStorage.setItem('dadosGrid', JSON.stringify(beneficiarios));

        preencherDadosBeneficiarios();

        $('#alteraBeneficiarioModal').modal('hide');
    });
})

function preencherDadosBeneficiarios() {
    $('#tabelaBeneficiarios tbody').empty();

    var beneficiarios = JSON.parse(localStorage.getItem('dadosGrid'));
    if (!beneficiarios) {
        return;
    }

    beneficiarios.forEach(function (beneficiario, index) {
        var linhaBeneficiario = $('<tr>');

        linhaBeneficiario.append($('<td>').text(beneficiario.NOME));
        linhaBeneficiario.append($('<td>').text(geraCPFFormatado(beneficiario.CPF)));

        var tdBotoes = $('<td>');
        var botaoAlterar = $('<button>').addClass('btn btn-primary').text('Alterar');
        var botaoExcluir = $('<button>').addClass('btn btn-danger').css('margin-left', '10px').text('Excluir');

        botaoAlterar.click(function () {
            const beneficiarios = JSON.parse(localStorage.getItem('dadosGrid'));
            const beneficiario = beneficiarios[index];
            $('#formAlteraBeneficiario #Nome').val(beneficiario.NOME);
            const inputCPF = $('#formAlteraBeneficiario #CPF');
            inputCPF.val(beneficiario.CPF);
            formatarCPF(inputCPF[0]);
            $('#formAlteraBeneficiario #Id').val(beneficiario.ID);
            $('#alteraBeneficiarioModal').modal('show');
        });
        botaoExcluir.click(function () {
            if (confirm('Tem certeza que deseja excluir este beneficiário?')) {
                var beneficiarios = JSON.parse(localStorage.getItem('dadosGrid'));
                beneficiarios.splice(index, 1);
                localStorage.setItem('dadosGrid', JSON.stringify(beneficiarios));

                preencherDadosBeneficiarios();
            }
        });

        tdBotoes.append(botaoAlterar);
        tdBotoes.append(botaoExcluir);
        tdBotoes.append('</td>');

        linhaBeneficiario.append(tdBotoes);

        $('#tabelaBeneficiarios tbody').append(linhaBeneficiario);
    });
}

function SetaBeneficiarioLocalStorage(nome, cpf, id) {
    let dadosGrid = localStorage.getItem('dadosGrid');
    if (!dadosGrid) {
        dadosGrid = [];
    } else {
        dadosGrid = JSON.parse(dadosGrid);
    }

    const novoDado = {
        "ID": id,
        "NOME": nome,
        "CPF": cpf
    };
    dadosGrid.push(novoDado);

    localStorage.setItem('dadosGrid', JSON.stringify(dadosGrid));
}

function formatarCPF(input) {
    let valor = input.value.replace(/\D/g, '');
    valor = geraCPFFormatado(valor);

    input.value = valor;
    validateCPF(input)
}

function geraCPFFormatado(valor) {
    valor = valor.replace(/(\d{3})(\d)/, '$1.$2');
    valor = valor.replace(/(\d{3})(\d)/, '$1.$2');
    valor = valor.replace(/(\d{3})(\d{2})$/, '$1-$2');

    return valor
}

function validateCPF(input) {
    let cpfSemPontuacao = input.value.replace(/[^\d]/g, '');
    let cpfInput = input.parentElement;
    let cpfValidationMessage = cpfInput.querySelector('#cpfValidationMessage');

    if (cpfSemPontuacao.length !== 11) {
        cpfValidationMessage.innerText = 'CPF inválido';
        return false;
    }

    let soma = 0;
    for (let i = 0; i < 9; i++) {
        soma += parseInt(cpfSemPontuacao.charAt(i)) * (10 - i);
    }

    let resto = soma % 11;
    let digitoVerificador1 = (resto < 2) ? 0 : 11 - resto;

    soma = 0;
    for (let i = 0; i < 10; i++) {
        soma += parseInt(cpfSemPontuacao.charAt(i)) * (11 - i);
    }

    resto = soma % 11;
    let digitoVerificador2 = (resto < 2) ? 0 : 11 - resto;

    if (parseInt(cpfSemPontuacao.charAt(9)) !== digitoVerificador1 || parseInt(cpfSemPontuacao.charAt(10)) !== digitoVerificador2) {
        cpfValidationMessage.innerText = 'CPF inválido';
        return false;
    }

    cpfValidationMessage.innerText = '';
    return true;
}

function formatarCEP(input) {
    let valor = input.value.replace(/\D/g, '');

    valor = geraCEPFormatado(valor);

    input.value = valor;
}

function geraCEPFormatado(valor) {
    if (valor.length > 5) {
        valor = valor.substring(0, 5) + '-' + valor.substring(5, valor.length);
    }

    return valor;
}

function ModalDialog(titulo, texto) {
    var random = Math.random().toString().replace('.', '');
    var texto = '<div id="' + random + '" class="modal fade">                                                               ' +
        '        <div class="modal-dialog">                                                                                 ' +
        '            <div class="modal-content">                                                                            ' +
        '                <div class="modal-header">                                                                         ' +
        '                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>         ' +
        '                    <h4 class="modal-title">' + titulo + '</h4>                                                    ' +
        '                </div>                                                                                             ' +
        '                <div class="modal-body">                                                                           ' +
        '                    <p>' + texto + '</p>                                                                           ' +
        '                </div>                                                                                             ' +
        '                <div class="modal-footer">                                                                         ' +
        '                    <button type="button" class="btn btn-default" data-dismiss="modal">Fechar</button>             ' +
        '                                                                                                                   ' +
        '                </div>                                                                                             ' +
        '            </div><!-- /.modal-content -->                                                                         ' +
        '  </div><!-- /.modal-dialog -->                                                                                    ' +
        '</div> <!-- /.modal -->                                                                                        ';

    $('body').append(texto);
    $('#' + random).modal('show');
}