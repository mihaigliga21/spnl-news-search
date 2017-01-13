function MakeSearch() {

    $('#sectionWaiting').show();

    var query = $('#queryArticleTitle').val();
    var market = $('#Select_market').val();
    
    if (market === '')
        market = 'en-US';

    console.log(market);
    console.log(query);
    $.ajax({
        type: 'GET',
        url: '/api/News/searchnews?query=' + query + '&market=' + market
    }).done(function(data) {
        console.log(data);

        $('#sectionWaiting').hide();

        $('#articles').empty();

        if(data.length === 0)
            $('#articles').append('<li><b>Nothing have been found. I\'m sorry!!</b></li>');

        jQuery.each(data, function (i, val) {
            $('#articles').append('<li> <b>Title:</b> ' + val.Title +
            '<br/><b>Description:</b> ' + val.Description +
            '<br/><b>Url:</b> ' + '<a href="' + val.Url + '" target="_blank">click to go to source</a>' +
            '<br/><b>Provider:</b> ' + val.Provider +
            '<br/><b>DatePublished:</b> ' + val.DatePublished +
            '<br/><b>Category:</b> ' + val.Category + '</li>');
        });

    }).fail(function(data) {
        console.log('eroare ' + data);
        $('#sectionWaiting').hide();
        $('#articles').append('<li><b>There was an error. I\'m sorry!!</b></li>');
    });
}