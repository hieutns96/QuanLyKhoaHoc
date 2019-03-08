function googleChart(json_data, chart_type, container, title) {
    google.charts.load('current', { packages: ['corechart'] });
    google.charts.setOnLoadCallback(function () {
        drawChart(json_data, container);
    });
    function drawChart(json_data, container) {
        var arr_data = JSON.parse(json_data);
        //console.log(arr_data);
        
        var data = google.visualization.arrayToDataTable(arr_data);
        
        var options = {}, chart = null;
        options.title = title !== undefined ? title : '';
        switch (chart_type) {
            case 'line':
                options = Object.assign(options, {
                    height: 500,
                    legend: { position: 'top', alignment: 'center' }
                });
                chart = new google.visualization.LineChart(document.getElementById(container));
                break;
            case 'pie':
                options = Object.assign(options, {
                    height: 500
                });
                chart = new google.visualization.PieChart(document.getElementById(container));
                break;
            default:
        }
        if (chart !== null) {
            chart.draw(data, options);
        }
    }
}
