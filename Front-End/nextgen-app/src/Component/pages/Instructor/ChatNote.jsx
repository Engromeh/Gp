import React from 'react'
// import Ellipse from "../../../assets/Ellipse 12.png"
// import Icon from "../../../assets/Icon.png"
// import { ReactComponent as Icon } from "../../../assets/Icon.png"
import questions from "../../../assets/questions.png"
function ChatNote() {
    return (

        <>
            <div className="d-flex flex-row-reverse align-items-center justify-content-between w-100 px-5 "
                style={{ marginTop: "3%" }}>
                <div className="btn d-flex align-items-center "
                    style={{ backgroundColor: "#FCD980", gap: "5px", borderRadius: "9px", height: "40px", marginBottom: "4%" }}>
                    <p className=" align-items-center mt-3 fw-bold" style={{ color: '#102D4C' }}>
                        الشات خاص
                    </p>
                    <img src={questions} alt="icon" style={{ width: "30px", gap: "5px" }} />
                </div>
            </div>

        </>
    )
}

export default ChatNote

