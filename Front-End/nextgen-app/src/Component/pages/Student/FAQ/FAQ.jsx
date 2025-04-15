import React from "react";

const faqQuestions = [
  { question: "كيف أبدأ؟", answer: "يمكنك البدء عبر التسجيل..." },
  { question: "ما هي شركة ALFARES؟", answer: "شركة ALFARES..." },
  { question: "ما هي رؤية ALFARES؟", answer: "الرؤية الخاصة بالشركة..." },
  { question: "ما هي الخدمات التي تقدمونها؟", answer: "نحن نقدم خدمات مثل..." },
  {
    question: "ما هي شروط وأحكام افتتاح الشركة؟",
    answer: "الشروط والأحكام الخاصة...",
  },
  {
    question: "كيف يعمل موقع ALFARES؟",
    answer: "موقع التسويق هو منصة إلكترونية...",
  },
  { question: "من هم مؤسسين ALFARES؟", answer: "معلومات عن المؤسسين..." },
  {
    question: "ما هي أهداف ALFARES؟",
    answer: "الأهداف الرئيسية لـ ALFARES...",
  },
  {
    question: "ما هي المزايا التي تقدمها شركة ALFARES؟",
    answer: "المزايا المقدمة تشمل...",
  },
];

const FAQ = () => {
  return (
    <>
      <div
        className="container py-5 text-white "
        style={{ direction: "rtl", marginTop: "6%" }}
      >
        <h2 className="text-center mb-4">الاسئلة الشائعة</h2>
        <div className="row">
          <div className="col-md-6">
            <div className="accordion" id="faqAccordionRight">
              {faqQuestions
                .slice(0, Math.ceil(faqQuestions.length / 2))
                .map((faq, index) => (
                  <div
                    className="accordion-item border-0 mb-3"
                    key={index}
                    style={{ backgroundColor: "#102D4C", borderRadius: "5px" }}
                  >
                    <h2 className="accordion-header">
                      <button
                        className="accordion-button collapsed text-white d-flex justify-content-between align-items-center"
                        type="button"
                        data-bs-toggle="collapse"
                        data-bs-target={`#faq${index}`}
                        aria-expanded="false"
                        aria-controls={`faq${index}`}
                        style={{
                          fontSize: "1rem",
                          fontWeight: "bold",
                          backgroundColor: "#102D4C",
                          boxShadow: "none",
                          marginLeft: "0",
                        }}
                      >
                        {faq.question}
                      </button>
                    </h2>
                    <div
                      id={`faq${index}`}
                      className="accordion-collapse collapse"
                      data-bs-parent="#faqAccordionRight"
                    >
                      <div className="answer-faq text-white p-3">
                        {faq.answer}
                      </div>
                    </div>
                  </div>
                ))}
            </div>
          </div>

          <div className="col-md-6">
            <div className="accordion" id="faqAccordionLeft">
              {faqQuestions
                .slice(Math.ceil(faqQuestions.length / 2))
                .map((faq, index) => (
                  <div
                    className=" border-0 mb-3"
                    key={index + 5}
                    style={{ backgroundColor: "#102D4C", borderRadius: "5px" }}
                  >
                    <h2 className="accordion-header">
                      <button
                        className="accordion-button collapsed text-white d-flex justify-content-between align-items-center"
                        type="button"
                        data-bs-toggle="collapse"
                        data-bs-target={`#faq${index + 5}`}
                        aria-expanded="false"
                        aria-controls={`faq${index + 5}`}
                        style={{
                          fontSize: "1rem",
                          fontWeight: "bold",
                          backgroundColor: "#102D4C",
                          boxShadow: "none",
                        }}
                      >
                        {faq.question}
                      </button>
                    </h2>
                    <div
                      id={`faq${index + 5}`}
                      className="accordion-collapse collapse"
                      data-bs-parent="#faqAccordionLeft"
                    >
                      <div className="answer-faq text-white p-3">
                        {faq.answer}
                      </div>
                    </div>
                  </div>
                ))}
            </div>
          </div>
        </div>
      </div>
    </>
  );
};

export default FAQ;
