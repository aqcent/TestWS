$(document).ready(
    function () {

        var searchTemplateContainer = document.querySelector('.js-search-results-template');
        var elasticTemplateContainer = document.querySelector('.js-elastic-search-template');
        if (searchTemplateContainer === null || searchTemplateContainer === undefined) {
            return;
        }

        var searchRequest = '';

        function encodeQueryData(data) {
            const ret = [];
            for (let d in data)
                ret.push(encodeURIComponent(d) + '=' + encodeURIComponent(data[d]));
            return ret.join('&');
        };

        var source = searchTemplateContainer.innerHTML;
        var elasticSource = elasticTemplateContainer.innerHTML;
        var template = Handlebars.compile(source);
        var elasticTemplate = Handlebars.compile(elasticSource);

        var movieList = {
            searchResults: [],
            num: 0
        };


        var formatter = new Intl.DateTimeFormat('ru', {
            hour: 'numeric',
            minute: 'numeric'
        });        

        $('.js-search-input').bind('input', function (e) {            

            if (event.keyCode == 13) 
                event.preventDefault();            

            searchRequest = $('.js-search-input').val();            

            if (!searchRequest) {                             
                $('.js-elastic-search').slideUp();                
                return;
            }


            clearTimeout(this.delay);
            this.delay = setTimeout(function () {

                if (!searchRequest) return;

                $.ajax({
                    url: '/tickets/processSearchRequest',
                    type: 'POST',
                    dataType: 'json',
                    contentType: 'application/json;charset=utf-8',
                    data: JSON.stringify({ 'searchRequest': searchRequest })
                }).done(function (movieListData) {

                    movieList.searchResults = [];
                    movieList.num = 0;

                    for (var i = 0; i < movieListData.length; i++) {

                        var timeSlotInfo = movieListData[i].AvailableTimeSlots;

                        var timeSlotsArr = [];
                        timeSlotInfo.forEach(timeslot => timeSlotsArr.push({
                            timeSlotURL: 'GetHallInfo?timeslotID=' + timeslot.TimeSlotID,
                            startTime: formatter.format(Date.parse(timeslot.StartTime)),
                            cost: timeslot.Cost
                        }));

                        var currentFilm = {
                            name: movieListData[i].Movie.Name,
                            imgUrl: movieListData[i].Movie.ImageURL,
                            duration: movieListData[i].Movie.Duration,
                            rating: movieListData[i].Movie.Rating,
                            minAge: movieListData[i].Movie.MinAge,
                            mainUrl: 'GetMovieByID?timeslotID=' + movieListData[i].Movie.Id,
                            timeSlots: []
                        };

                        currentFilm.timeSlots.push(timeSlotsArr);

                        movieList.searchResults.push(currentFilm);
                        movieList.num = i + 1;

                    }                    

                    var elasticResultHTML = elasticTemplate(movieList);
                    $('.js-elastic-search').html(elasticResultHTML);

                    document.querySelectorAll('.js-list-item').forEach(function (elem) {
                        let str = elem.innerText;
                        elem.innerHTML = postMark(str, elem.innerText.search(new RegExp(searchRequest, 'i')), searchRequest.length);                        
                    });

                    $('.js-elastic-search').slideDown();

                    
                    e.target.onkeyup = (key) => {

                        key.preventDefault();
                        
                        let selected = document.querySelector('.js-list-item.select');                                                                                               
                        
                        if (key.key == 'ArrowDown') {                              

                            if (selected == null) {
                                selected = document.querySelectorAll('.js-list-item')[0];
                                selected.classList.add('select');
                                return;
                            }

                            selected.classList.remove('select');

                            if (selected.nextElementSibling != null) {                                
                                selected = selected.nextElementSibling;                                
                            }
                            else
                                selected = document.querySelectorAll('.js-list-item')[0];

                            selected.classList.add('select');                            
                        }

                        if (key.key == 'ArrowUp') {

                            if (selected == null) {
                                selected = document.querySelector('.list-group').lastElementChild;                                
                                selected.classList.add('select');
                                return;
                            }

                            selected.classList.remove('select');                            
                            

                            if (selected.previousElementSibling != null) {
                                selected = selected.previousElementSibling;
                            }
                            else
                                selected = document.querySelector('.list-group').lastElementChild; 

                            selected.classList.add('select');                            
                        }

                        
                    }

                }).fail(() => {
                    console.log('Fail request');
                });
            }.bind(this), 800);
        });
    });


function postMark(string, pos, len) {
    return string.slice(0, pos) + '<b>' + string.slice(pos, pos + len) + '</b>' + string.slice(pos + len);
}