
 {
    var name = $("#chartUser").val();
    var address = "/Users/getUserData?id=";
    address += encodeURIComponent(name);
    $.ajax({
        cache: false,
        type: "GET",
        url: address,
        dataType: 'json',
        success: function (result) {
            var data1 = [];
            for (var i = 0; i < result.length; i++) {
                data1.push({ label: result[i].label, quantity: result[i].quantity });
            }
            updateBar(data1);
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert('Failed to retrieve data.');
        }
    });
                
};
           
$.ajax({
    cache: false,
    type: "GET",
    url: "/Users/getCategoryData",
    dataType: 'json',
    success: function (result) {
        var data = [];
        for (var i = 0; i < result.length; i++) {
            data.push({ label: result[i].label, quantity: result[i].quantity });
            //$("#chartID").append(result[i].label + result[i].quantity)
        }
        d3BarChart(data);


    },
    error: function (xhr, ajaxOptions, thrownError) {
        alert('Failed to retrieve data.');
    }
});


var margin = { top: 10, right: 10, bottom: 90, left: 10 };

var width = 960 - margin.left - margin.right;

var height = 500 - margin.top - margin.bottom;

var xScale = d3.scale.ordinal().rangeRoundBands([0, width], .03)

var yScale = d3.scale.linear()
      .range([height, 0]);


var xAxis = d3.svg.axis()
        .scale(xScale)
        .orient("bottom");

               



function d3BarChart(dataSet) {

    var barChart = d3.select("#chartID").append("svg")
                   .attr("width", width + margin.left + margin.right)
                   .attr("height", height + margin.top + margin.bottom)
                   .append("g").attr("class", "container")
                   .attr("transform", "translate(" + margin.left + "," + margin.top + ")");
                    
    xScale.domain(dataSet.map(function (d) { return d.label; }));
    yScale.domain([0, d3.max(dataSet, function (d) { return d.quantity; })]);            

    var xAxis_g = barChart.append("g")
               .attr("class", "x axis")
               .attr("transform", "translate(0," + (height) + ")")
               .call(xAxis)
               .selectAll("text");


    barChart.selectAll(".bar")
        .data(dataSet)
        .enter()
        .append("rect")
        .attr("class", "bar")
        .attr("x", function (d) { return xScale(d.label); })
        .attr("width", xScale.rangeBand())
        .attr("y", function (d) { return yScale(d.quantity); })
        .attr("height", function (d) { return height - yScale(d.quantity); });

    barChart.selectAll(".text")
      .data(dataSet)
      .enter()
      .append("text")
      .attr("class", "label")
      .attr("x", (function (d) { return xScale(d.label) + xScale.rangeBand() / 2; }))
      .attr("y", function (d) { return yScale(d.quantity); })
      .attr("dy", "2.0em")
      .text(function (d) { return d.quantity; });

}
function updateBar(dataSet) {
    barChart.exit().remove();
}
        

       $.ajax({
           cache: false,
           type: "GET",
           url: "/Users/getNumOfRecipes",
           dataType: 'json',
           success: function (result) {
               var data = [];
               for (var i = 0; i < result.length; i++) {
                   data.push({ label: result[i].label, value: result[i].quantity });
               }
               pieBuild(data);


           },
           error: function (xhr, ajaxOptions, thrownError) {
               alert('Failed to retrieve data.');
           }
       });
        



function pieBuild(piedata) {

    var width = 400,
       height = 400,
       radius = 200
    colors = d3.scale.ordinal()
        .range(['#595AB7', '#A57706', '#D11C24', '#C61C6F', '#BD3613', '#2176C7', '#259286', '#738A05']);

    var pie = d3.layout.pie()
        .value(function (d) {
            return d.value;
        });

    var arc = d3.svg.arc()
            .outerRadius(radius);

    var myChart = d3.select('#pie1').append('svg')
        .attr('width', width)
        .attr('height', height)
        .append('g')
        .attr('transform', 'translate(' + (width - radius) + ',' + (height - radius) + ')')
        .selectAll('path').data(pie(piedata))
        .enter().append('g')
        .attr('class', 'slice');



    var slices = d3.selectAll('g.slice')
        .append('path')
        .attr('fill', function (d, i) {
            return colors(i);
        })
        .attr('d', arc);

    var text = d3.selectAll('g.slice')
        .append('text')
        .text(function (d, i) {
            return d.data.label + " , " + d.data.value;
        })
        .attr('text-ancor', 'end')
        .attr('fill', 'white')
        .attr('transform', function (d) {
            d.innerRadius = 0;
            d.outerRadius = radius;
            return 'translate(' + arc.centroid(d) + ')'

        });
}