import { useNavigate } from "react-router-dom"
import { CommentItem } from "./comment-item"
import { COMMENT_DETAILS } from "../../utility/routes/app-paths";

export const DisplayComments = ({ comments, ...props }) => {
    const navigate = useNavigate();

    const handleOnClick = (id) => {
        // console.log('COMMENT ID');
        // console.log(id);
        navigate(`${COMMENT_DETAILS.replace(':id', id)}`)
    }
    return (
        <div>
            {comments.map(item => {
                return <CommentItem key={item.id} item={item} onClick={handleOnClick} />
            })}
        </div>
    )
}