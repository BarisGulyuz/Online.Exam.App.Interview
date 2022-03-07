
$(document).ready(() => {
    const url = 'https://localhost:44384/Exam/GetExamQuestions'
    const examId = localStorage.getItem('examId')
    const questionCount = localStorage.getItem('questionCount')

    if (localStorage.getItem('questionNumber') == null) {
        localStorage.setItem('questionNumber', 0);
    }

    let i = localStorage.getItem('questionNumber')
    if (Number(i) == Number(questionCount - 1)) {
        $('.q-next').addClass('d-none')
        $('.finish').removeClass('d-none')
    }
    else {
        $('.q-next').removeClass('d-none')
        $('.finish').addClass('d-none')
    }
    if (Number(i) == 0) {
        $('.q-prev').addClass('d-none')
    }
    else {
        $('.q-prev').removeClass('d-none')
    }


    $('.q-next').click(function (e) {
        e.preventDefault()
        i = Number(parseInt(i) + 1)
        localStorage.setItem('questionNumber', i.toString())
        window.location.replace(url + '?examId=' + examId + '&questionNumber=' + i)
    })

    $('.q-prev').click(function (e) {
        e.preventDefault()
        i = Number(parseInt(i) - 1)
        if (i < 0) {
            i = 0
        }
        localStorage.setItem('questionNumber', i.toString())
        window.location.replace(url + '?examId=' + examId + '&questionNumber=' + i)
    })

    let radios = $(".radio");
    radios.change(function () {
        radios.each(function () {
            if ($(this).is(":checked")) {
                $(this).parent().addClass("selected");
                localStorage.setItem(`question${i}`, $(this).val())
            }
            else {
                $(this).parent().removeClass("selected");
            }

        });
    });

    if (localStorage.getItem(`question${i}`) != null) {
        radios.each(function () {
            if ($(this).val() == localStorage.getItem(`question${i}`)) {
                $(this).prop('checked', true)
                $(this).parent().addClass("selected");
            }
        });
    }

    $('#finish').click((e) => {
        e.preventDefault()
        let answers = []
        for (let i = 0; i <= questionCount - 1; i++) {
            let questionNum = i
            let answer = localStorage.getItem(`question${i}`)
            answers.push({ questionNum, answer })
            localStorage.removeItem(`question${i}`)
            localStorage.removeItem('questionCount')
        }
        $.ajax({
            type: 'POST',
            url: '/Exam/FinishExam',
            data: { examId: Number(examId), questionAnswers: answers },
            success: function (data) {
                console.log(data)
                window.location.replace('https://localhost:44384/Exam/ResultPage?point=' + data.point + '&status=' + data.successStatus)
                localStorage.setItem('questionNumber', 0)
            }
        })
      
    })
})