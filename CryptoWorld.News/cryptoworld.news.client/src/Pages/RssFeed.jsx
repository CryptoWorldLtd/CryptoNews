import React, { useState } from 'react';
import axios from 'axios';

const RssFeed = () => {
    const [feedItems, setFeedItems] = useState([]); 
    const [error, setError] = useState(null);
    const [loading, setLoading] = useState(false);

    const fetchFeed = async () => {
        setLoading(true);
        setError(null);
        try {
            const response = await axios.get('https://localhost:7249/news/rssFeed');
            console.log("Response data:", response.data);
            setFeedItems(response.data);
        } catch (error) {
            if (error.response) {
                console.error('Response error', error.response.data);
                setError(`Error fetching the RSS feed: ${error.response.data}`);
            } else if (error.request) {
                console.error('Request error', error.request);
                setError('Error fetching the RSS feed: No response from server');
            } else {
                console.error('General error', error.message);
                setError(`Error fetching the RSS feed: ${error.message}`);
            }
        } finally {
            setLoading(false);
        }
    };

    const parseDate = (dateString) => {
        const [day, month, year] = dateString.split('.');
        return new Date(`${year}-${month}-${day}`).toLocaleDateString();
    };

    return (
        <div>
            <button onClick={fetchFeed}>Fetch Feed</button>
            {loading && <p>Loading...</p>}
            {error && <p>{error}</p>}
            <ul>
                {feedItems.map((item, index) => (
                    <li key={index}>
                        <h3><a href={item.link} target="_blank" rel="noopener noreferrer">{item.title}</a></h3>
                        <p><strong>Published on: </strong>{parseDate(item.publishDate)}</p>
                        {item.description && <div dangerouslySetInnerHTML={{ __html: item.description }} />}
                        {item.link && <div dangerouslySetInnerHTML={{ __html: item.content }} />}
                        {item.copyright && <p><strong>Source: </strong>{item.copyright}</p>}
                    </li>
                ))}
            </ul>
        </div>
    );
};

export default RssFeed;