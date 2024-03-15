$(() => {
    loadQuestions();
    console.log('done');
    function loadQuestions() {
        $(".offset-md-2").remove();
        $.get("/home/getquestions", function (questions) {
            questions.forEach(function (question) {
                $.get("/home/getinfoforquestion", question, function (vm) {
                    $(".row").append(`
                  <div class="card card-body bg-light">
                    <div>
                        <h4>
                            <a href="/home/viewquestion?id=${question.id}">${question.title}</a>
                        </h4>
                    </div>
                    <div>
                 <span>Tags: </span>
                 ${vm.tags.map(function (tag) {
                        return `<span class="badge bg-primary">${tag.description}</span>`;
                    }).join(' ')}
                 <div style="margin-top:10px;">${question.text}</div>
                 <span>Likes: </span>
                 <span>${vm.likes}</span>
                 <br>
                 <span>${vm.answerCount} answer(s)</span>
                 </div>
                  </div>  
                    `)
                })
            });
        })
    }

})