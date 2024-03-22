
$(document).ready(function () {

    if (document.getElementById("gridClientes"))
        $('#gridClientes').jtable({
            title: 'Clientes',
            paging: true, //Enable paging
            pageSize: 5, //Set page size (default: 10)
            sorting: true, //Enable sorting
            defaultSorting: 'Nome ASC', //Set default sorting
            actions: {
                listAction: urlClienteList,
            },
            fields: {
                Nome: {
                    title: 'Nome',
                    width: '40%'
                },
                Email: {
                    title: 'Email',
                    width: '40%'
                },
                Botoes: {
                    title: '',
                    sorting: false,
                    width: '20%',
                    display: function (data) {
                        return '<div class="btn-group" role="group" aria-label="Ações">' +
                            '<button onclick="window.location.href=\'' + urlAlteracao + '/' + data.record.Id + '\'" class="btn btn-primary btn-sm" style="margin-right: 10px">Alterar</button>' +
                            '<button onclick="confirmarExclusao(' + data.record.Id + ')" class="btn btn-danger btn-sm">Excluir</button>' +
                            '</div>';
                    }
                },
            }
        });

    //Load student list from server
    if (document.getElementById("gridClientes"))
        $('#gridClientes').jtable('load');
})

function confirmarExclusao(clienteId) {
    if (confirm('Tem certeza que deseja excluir este cliente?')) {
        $.ajax({
            url: urlExcluir + '/' + clienteId,
            type: 'DELETE',
            success: function (response) {
                $('#gridClientes').jtable('reload');
            },
            error: function (xhr, status, error) {
                console.error('Erro ao excluir cliente:', error);
            }
        });
    }
}