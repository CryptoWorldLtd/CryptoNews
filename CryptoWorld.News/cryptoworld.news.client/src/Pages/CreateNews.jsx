import React, { useState } from 'react';
import './CreateNews.css';
import axios from 'axios';

const CreateNews = () => {
  const [title, setTitle] = useState("");
  const [content, setContent] = useState("");
  const [imageUrl, setImageUrl] = useState("");
  const [datePublished, setDatePublished] = useState("");
  const [error, setError] = useState(null);

  const handleTitleChange = (event) => {
    setTitle(event.target.value);
  };

  const handleContentChange = (event) => {
    setContent(event.target.value);
  };

  const handleImageUrlChange = (event) => {
    setImageUrl(event.target.value);
  };

  const handleDatePublishedChange = (event) => {
    setDatePublished(event.target.value);
  };

  const handleSubmit = async () => {
    setError(null);
    try {
      console.log('Submitting:', { title, content, imageUrl, datePublished });
      const response = await axios.post('https://localhost:7249/createNews', {
        title,
        content,
        imageUrl,
        datePublished
      }, {
        headers: {
          'Content-Type': 'application/json'
        }
      });
      console.log('Response:', response.data);
      navigate("/createNews");
    } catch (error) {
      console.error('Error:', error.response ? error.response.data : error.message);
      setError(error.response?.data?.message || "Registration failed");
    }
  };

  return (
    <div className='container'>
      <div className="header">
        <div className='text'>Create News</div>
        <div className='underline'></div>
      </div>
      <div className='inputs'>
        <div className='input-group'>
          <div className='input'>
            <input 
              type='text' 
              placeholder='Title' 
              value={title} 
              onChange={handleTitleChange} 
            />
          </div>
        </div>
        
        <div className='input-group'>
          <div className='input'>
            <textarea
              placeholder='Content' 
              value={content} 
              onChange={handleContentChange} 
            />
          </div>
        </div>

        <div className='input-group'>
          <div className='input'>
            <input 
              type='text' 
              placeholder='ImageUrl' 
              value={imageUrl}
              onChange={handleImageUrlChange}
            />
          </div>
        </div>

        <div className='input-group'>
          <div className='input'>
            <input 
              type='text' 
              placeholder='Date format: "dd.MM.yyyy HH:mm:ss"' 
              value={datePublished}
              onChange={handleDatePublishedChange}
            />
          </div>
        </div>

        <div className='submit-container'>
          <button onClick={handleSubmit}>
            Submit News
          </button>
        </div>
        {error && <div className="error">{error}</div>}
      </div>
    </div>
  );
}

export default CreateNews;
