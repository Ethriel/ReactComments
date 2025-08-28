import { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';

import axios from 'axios';
import { Form, Input, Button, Flex, Pagination } from 'antd';
import 'react-quill-new/dist/quill.snow.css';

import { AddComment } from '../comment/add-comment';
import { DisplayComments } from '../comment/display-comments';
import { getCurrentUser } from '../../utility/handle-current-user/get-current-user'

const testComments = [
    {
        id: 1,
        text: "<p>text</p>",
        personName: "Jhon"
    },
    {
        id: 2,
        text: "<p>text</p><p><strong>bold</strong></p>",
        personName: "Dave"
    },
    {
        id: 3,
        text: "<p>text</p><p><strong>bold</strong></p><p><i>italic</i></p>",
        personName: "Steeve"
    },
    {
        id: 4,
        text: "<p>text</p><p><strong>bold</strong></p><p><i>italic</i></p><code>code</code>",
        personName: "Kyle"
    },
    {
        id: 5,
        text: "<p>text</p><p><strong>bold</strong></p><p><i>italic</i></p><code>code</code><p>DIRTY</p><script src=''>DIRTY SCRIPT</script>",
        personName: "Anna"
    }
]

export const MainComponent = () => {
    const [currentPage, setCurrentPage] = useState(1);
    const [comments, setComments] = useState([]);
    const [hasComments, setHasComments] = useState(false);

    const commentsPerPage = 2;
    const indexOfLastComment = currentPage * commentsPerPage;
    const indexOfFirstComment = indexOfLastComment - commentsPerPage;
    const currentComments = comments.slice(indexOfFirstComment, indexOfLastComment);
    const testCurrentComments = testComments.slice(indexOfFirstComment, indexOfLastComment);

    const getTopLevelComments = () => {
        axios.get('api/comment/list-top-level', {
            headers: {
                'Content-Type': 'application/json'
            }
        })
            .then((response) => {
                const data = response.data;
                setHasComments(data.length > 0)
                setComments(data);
            })
            .catch((error) => {
                console.error('ERROR', error);
                // openErrorNotification(error.response.data);
            });
    }

    useEffect(() => {
        getTopLevelComments();
    }, [])

    const handlePageChange = (page) => {
        setCurrentPage(page);
    };


    return (
        <>
            <Flex gap="middle" align="center" justify='center' vertical>
                {hasComments ? (
                    <>
                        <DisplayComments comments={currentComments} />
                        <Pagination
                            current={currentPage}
                            pageSize={commentsPerPage}
                            total={testComments.length}
                            onChange={handlePageChange}
                            style={{ marginTop: '20px', textAlign: 'center' }}
                        />
                    </>
                ) : (
                    <h2>No comments to show</h2>
                )}
            </Flex>
        </>
    );
};