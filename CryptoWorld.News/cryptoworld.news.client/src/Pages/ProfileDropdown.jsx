import React, { useState, useEffect, useRef } from 'react';
import { useNavigate } from 'react-router-dom';
import './ProfileDropdown.css';
import user from '../assets/user.png';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faEnvelope, faLock, faSignOut, faNewspaper } from '@fortawesome/free-solid-svg-icons';


const Profile = () => {
    const [open, setOpen] = useState(false);
    const menuRef = useRef();
    const navigate = useNavigate();

    useEffect(() => {
        const handler = (e) => {
            if (!menuRef.current.contains(e.target)) {
                setOpen(false);
            }
        };

        document.addEventListener("mousedown", handler);

        return () => {
            document.removeEventListener("mousedown", handler);
        };
    }, []);

    const handleNavigation = (path) => {
        setOpen(false);
        navigate(path);
    };

    return (
        <div className='Profile'>
            <div className='menu-container' ref={menuRef}>
                <div className='menu-trigger' onClick={() => setOpen(!open)}>
                    <img src={user} alt="User" />
                </div>

                <div className={`dropdown-menu ${open ? 'active' : 'inactive'}`}>
                    <ul>
                        <DropdownItem icon={faNewspaper} text="My news" onClick={() => handleNavigation('/MyNews')} />
                        <DropdownItem icon={faEnvelope} text="Change e-mail" onClick={() => handleNavigation('/ChangeEmail')} />
                        <DropdownItem icon={faLock} text="Change password" onClick={() => handleNavigation('/ChangePassword')} />
                        <DropdownItem icon={faSignOut} text="Logout" onClick={() => handleNavigation('/Logout')} />
                    </ul>
                </div>
            </div>
        </div>
    );
};

const DropdownItem = ({ icon, text, onClick }) => {
    return (
        <li className='dropdownItem' onClick={onClick}>
            <i>
                <FontAwesomeIcon icon={icon} />
            </i>
            <span>{text}</span>
        </li>
    );
};

export default Profile;