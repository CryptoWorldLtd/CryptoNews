import React, { useState } from 'react';
import './Register.css';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faUser, faEnvelope } from '@fortawesome/free-regular-svg-icons';
import { faLock } from '@fortawesome/free-solid-svg-icons';
import axios from 'axios';

const Register = () => {
  const [action, setAction] = useState("Register");
  const [username, setName] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const [error, setError] = useState(null);

  const handleUsernameChange = (event) => {
    setName(event.target.value);
  };

  const handleEmailChange = (event) => {
    setEmail(event.target.value);
  };

  const handlePasswordChange = (event) => {
    setPassword(event.target.value);
  };
  const handleConfirmPasswordChange = (event) => {
    setConfirmPassword(event.target.value);
  };

  const handleSubmit = async () => {
    try {
      const response = await axios.post('https://localhost:61600/account/register', {
        username,
        email,
        password,
        confirmPassword
      });
      console.log(response.data);
      // Handle success (e.g., redirect or show success message)
    } catch (error) {
      console.error(error);
      setError("Registration failed");
      // Handle error (e.g., show error message)
    }
  };

  return (
    <div className='container'>
      <div className="header">
        <div className='text'>{action}</div>
        <div className='underline'></div>
      </div>
      <div className='inputs'>
        <div className='input-group'>
          <div className='input'>
            <i>
              <FontAwesomeIcon icon={faUser} />
            </i>
            <input 
              type='text' 
              placeholder='Name' 
              value={username} 
              onChange={handleUsernameChange} 
            />
          </div>
        </div>
        
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
          <input 
            type='password' 
            placeholder='Password' 
            value={password}
            onChange={handlePasswordChange}
          />
        </div>
        <div className='input'>
          <i>
            <FontAwesomeIcon icon={faLock} />
          </i>
          <input 
            type='confirmPassword' 
            placeholder='Confirm Password' 
            value={confirmPassword}
            onChange={handleConfirmPasswordChange}
          />
        </div>
      
        <div className='submit-container'>
          <div 
            className={action === "Login" ? "submit gray" : "submit"} 
            onClick={handleSubmit}
          >
            Register
          </div>
        </div>
        {error && <div className="error">{error}</div>}
      </div>
    </div>
  );
}

export default Register;
