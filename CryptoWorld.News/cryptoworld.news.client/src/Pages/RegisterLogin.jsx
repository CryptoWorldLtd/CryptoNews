import React, { useState } from 'react';
import './RegisterLogin.css';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faUser, faEnvelope } from '@fortawesome/free-regular-svg-icons';
import { faLock } from '@fortawesome/free-solid-svg-icons';

const RegisterLogin = () => {
    
  const [action, setAction] = useState("Register");
  const [name, setName] = useState("");
  const [email, setEmail] = useState("");

  const handleNameChange = (event) => {
    setName(event.target.value);
  };

  const handleEmailChange = (event) => {
    setEmail(event.target.value);
  };

  return (
    <div className='container'>
      <div className="header">
        <div className='text'>{action}</div>
        <div className='underline'></div>
      </div>
      <div className='inputs'>
        {action==="Login"? <div></div> : 
          <div className='input-group'>
            <div className='input'>
              <i>
                <FontAwesomeIcon icon={faUser} />
              </i>
              <input 
                type='text' 
                placeholder='Name' 
                value={name} 
                onChange={handleNameChange} 
              />
            </div>
          </div>
        }
        <div className='input-group'>
          <div className='input'>
            <i>
              <FontAwesomeIcon icon={faEnvelope} />
            </i>
            <input 
              type='email' 
              placeholder='Email' 
              value={email} 
              onChange={handleEmailChange} 
            />
          </div>
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
export default RegisterLogin;
