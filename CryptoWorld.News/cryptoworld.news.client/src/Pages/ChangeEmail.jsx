import React, { useState } from 'react';
import { useNavigate } from "react-router-dom";
import './Register.css';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faEnvelope } from '@fortawesome/free-regular-svg-icons';
import axios from 'axios';

const ChangeEmail = () => {
  const [action] = useState("Change Email");
  const [email] = useState("");
  const [password] = useState("");
  const [confirmPassword] = useState("");
  const [error, setError] = useState(null);
    
  const navigate = useNavigate();

  const handleSubmit = async () => {
    setError(null); 
    try {
      const response = await axios.post('https://localhost:7249/account/edit', {
        email,
        newEmail,
        confirmNewEmail
      }, {
        headers: {
          'Content-Type': 'application/json'
        }
      });
        navigate("/Login");
        
    } catch (error) {
      console.error('Error:', error.response ? error.response.data : error.message);
      setError(error.response?.data?.message || "Email change failed");
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
              <FontAwesomeIcon icon={faEnvelope} />
            </i>
            <input 
              type='email' 
              placeholder='Current Email' 
              value={email} 
            />
          </div>
        </div>
        <div className='input'>
           <i>
              <FontAwesomeIcon icon={faEnvelope} />
            </i>
          <input 
            type='email' 
            placeholder='New Email'
            value={password}
          />
        </div>
        <div className='input'>
          <i>
            <FontAwesomeIcon icon={faEnvelope} />
          </i>
          <input 
            type='email' 
            placeholder='Confirm new Email' 
            value={confirmPassword}
          />
        </div>
        <div className='submit-container'>
          <div 
            className={action === "Login" ? "submit gray" : "submit"} 
            onClick={(handleSubmit)}
          >
            Save
          </div>
        </div>
        {error && <div className="error">{error}</div>}
      </div>
    </div>
  );
}

export default ChangeEmail;