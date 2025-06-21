(function ($) {
    "use strict";

    // Функция для форматирования чисел (уже есть в вашем коде)
    function formatNumber(num) {
        if (num >= 1000000) return (num / 1000000).toFixed(1) + 'm';
        if (num >= 1000) return (num / 1000).toFixed(1) + 'k';
        return num.toString();
    }

    // Функция для обновления счётчиков (уже есть в вашем коде)
    function updateCounters(data) {
        $('[data-counter="users"]').text(formatNumber(data.userCount));
        $('[data-counter="books"]').text(formatNumber(data.bookCount));
        $('[data-counter="sales"]').text(formatNumber(data.saleCount));
        $('[data-counter="orders"]').text(formatNumber(data.orderCount));
    }

    

    

    // Загрузка данных дашборда (уже есть в вашем коде)
    function loadDashboardData() {
        $.ajax({
            url: '/api/dashboard',
            type: 'GET',
            success: function (data) {
                updateCounters(data);
            },
            error: function (xhr) {
                console.error('Error loading dashboard data:', xhr.responseText);
            }
        });
    }

    // SignalR подключение (уже есть в вашем коде)
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/hubs/dashboard")
        .withAutomaticReconnect()
        .build();

    connection.on("ReceiveDashboardUpdate", function (data) {
        updateCounters(data);
    });

    function loadOpenInvoices() {
        $.ajax({
            url: '/api/dashboard/invoices/open',
            method: 'GET',
            success: function (invoices) {
                const tbody = $('table tbody');
                tbody.empty(); // Очистка

                invoices.forEach(invoice => {
                    const badgeClass = invoice.status === "Paid"
                        ? "bg-success"
                        : invoice.status === "Unpaid"
                            ? "bg-warning"
                            : "bg-danger";

                    const row = `
<tr>
    <td>${invoice.ClientName}</td>
    <td>${invoice.Date}</td>
    <td>${invoice.InvoiceNumber}</td>
    <td>${invoice.Amount}</td>
    <td><div class="badge badge-pill ${badgeClass}">${invoice.Status}</div></td>
    <td>
        <button class="btn btn-sm btn-outline-primary">Copy</button>
    </td>
</tr>
`;

                    tbody.append(row);
                });
            },
            error: function (xhr) {
                console.error('Ошибка загрузки заказов:', xhr.responseText);
            }
        });
    }




    connection.start()
        .then(function () {
            console.log("SignalR connected");
            loadDashboardData();
            loadOpenInvoices(); // Вызываем загрузку открытых счетов при подключении
        })
        .catch(function (err) {
            console.error("SignalR connection error:", err.toString());
        });

    connection.onreconnected(function () {
        console.log("SignalR reconnected");
        loadDashboardData();
        loadOpenInvoices(); // Повторно загружаем счета при переподключении
    });

})(jQuery);