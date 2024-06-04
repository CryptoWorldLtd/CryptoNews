import React, {useState} from 'react'
import './RegisterLogin.css'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faUser, faEnvelope } from '@fortawesome/free-regular-svg-icons';
import { faLock } from '@fortawesome/free-solid-svg-icons';

 const RegisterLogin = () => {
    
  const[action,setAction] = useState("Register");

  return (
    <div className='container'>
      <div className="header">
        <div className='text'>{action}</div>
        <div className='underline'></div>
      </div>
      <div className='inputs'>
        {action==="Login"? <div></div> : <div className='input'>
          <i>
            <FontAwesomeIcon icon={faUser} />
          </i>
          <input type='text' placeholder='Name' />
        </div>}
     
        <div className='input'>
          <i>
            <FontAwesomeIcon icon={faEnvelope} />
          </i>
          <input type='email' placeholder='Email' />
        </div>
        <div className='input'>
          <i>
            <FontAwesomeIcon icon={faLock} />
          </i>
          <input type='password' placeholder='Password' />
        </div>
        {action==="Register"?<div></div> : <div className='forgot-password'>Forgot password? <span>Click here</span></div>}
        <div className='submit-container'>
            <div className={action==="Login"?"submit gray" : "submit"} onClick={()=>{setAction("Register")}}>Register</div>
            <div className={action==="Register"?"submit gray" : "submit"} onClick={()=>{setAction("Login")}} >Login</div>
        </div>
      </div>
    </div>
  );
}
export default RegisterLogin