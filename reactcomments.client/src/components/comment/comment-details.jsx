import { useEffect, useState } from "react"
import { useNavigate, useParams } from "react-router-dom";

import axios from "axios";

import { CommentItem } from "./comment-item";
import { getCurrentUser } from "../../utility/handle-current-user/get-current-user";
import { Button, Flex } from "antd";
import { ADD_REPLY } from "../../utility/routes/app-paths";

export const CommentDetails = ({ ...props }) => {
    const [comment, setComment] = useState(null);

    const { id } = useParams();
    const navigate = useNavigate();

    const currentUser = getCurrentUser();

    const getCommentDetails = () => {
        axios.get(`/api/comment/comment-details`, {
            params: { id },
            headers: {
                'Content-Type': 'application/json'
            }
        })
            .then((response) => {
                const data = response.data;

                // console.log(data)

                setComment(data);
            })
            .catch((error) => {
                console.error('ERROR', error);
                openErrorNotification(error.response.data);
            });
    }

    const onAddReplyClick = () => {
        navigate(`${ADD_REPLY.replace(':id', id)}`)
    }

    const getMimeType = (base64String) => {
        if (base64String.startsWith('/9j/')) return 'image/jpeg';
        if (base64String.startsWith('iVBOR')) return 'image/png';
        return 'image/*';
    };

    let imageContents = comment && comment.imageAttachment ? comment.imageAttachment.contents : '';
    let imageType = comment === null ? 'image/*' : getMimeType(imageContents);
    const isSignedIn = currentUser === null ? false : currentUser.isSignedIn;
    const hasReplies =
        (comment === null) ? false :
            (comment.replies === null) ? false :
                (comment.replies.length > 0) ? true : false;

    useEffect(() => {
        getCommentDetails();
    }, [])

    return (
        <>
            {comment !== null && (
                <Flex gap="middle" align="start" justify='center' vertical>
                    <CommentItem
                        key={comment.id}
                        style={{ minWidth: 300, marginTop: '1%' }}
                        item={comment}
                        onClick={null} />
                    {
                        comment.imageAttachment !== null && (
                            <img src={`data:${imageType};base64,${imageContents}`} />
                        )
                    }
                    {
                        hasReplies && (
                            comment.replies.map((c) => {
                                imageContents = c && c.imageAttachment ? c.imageAttachment.contents : '';
                                imageType = c === null ? 'image/*' : getMimeType(imageContents);
                                const styles = { minWidth: 300, marginLeft: '2%' }
                                return <>
                                    <CommentItem
                                        style={styles}
                                        key={c.id}
                                        item={c}
                                        onClick={null} />
                                    {/* {
                                        c.imageAttachment !== null && (
                                            <img src={`data:${imageType};base64,${imageContents}`} />
                                        )
                                    } */}

                                </>

                            })
                        )
                    }
                    {
                        (
                            <Button
                                style={{ marginTop: '5px', marginBottom: '5px' }}
                                type='default'
                                disabled={!isSignedIn}
                                onClick={() => { onAddReplyClick() }}>
                                {isSignedIn ? 'Add reply' : 'Sign in to add reply'}
                            </Button>
                        )
                    }
                </Flex>
            )}
        </>
    )
}