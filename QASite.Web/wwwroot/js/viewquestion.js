$(() => {
    console.log('in');
    $("#like-question").on('click', function () {
        var id = $(this).data('question-id');
        console.log(id);
        $.post("/home/addtosession", { id });
    });
})