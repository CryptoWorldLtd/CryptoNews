import React, { useState, useEffect, useRef } from 'react';
import user from '../assets/user.png';
import { faEnvelope } from '@fortawesome/free-regular-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faLock, faSignOut } from '@fortawesome/free-solid-svg-icons';

const Profile = () => {
    const [open, setOpen] = useState(false);

    let menuRef = useRef();

    useEffect(() => {
        let handler = (e)=>{
            if(!menuRef.current.contains(e.target)){
                setOpen(false);
            }
        }

        document.addEventListener("mousedown", handler);

        return() =>{
            document.removeEventListener("mousedown", handler);
        }
    }, []);

    return(
        <div className='Profile'>
            <div className='menu-container' ref={menuRef}>
                <div className='menu-trigger' onClick={()=>{setOpen(!open)}}>
                    <img src={user}></img>
                </div>

                <div className={`dropdown-menu ${open? 'active' : 'inactive'}`}>
                    <ul>
                        <DropdownItem icon={faEnvelope} text = {"Change e-mail"}/>
                        <DropdownItem icon={faLock} text = {"Change password"}/>
                        <DropdownItem icon = {faSignOut} text = {"Logout"}/>
                    </ul>
                </div>
            </div>
        </div>
    )
}

const DropdownItem = (props) => {
return(
        <li className='dropdownItem'>
             <i>
              <FontAwesomeIcon icon={props.icon} />
            </i>
            <a>{props.text}</a>
        </li>
    );
}

export default Profile;