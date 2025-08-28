import DOMpurify from 'dompurify'
import { Card } from 'antd';
import { formatDate } from '../../utility/format-date';

export const CommentItem = ({ item, onClick, style, ...props }) => {
    const clean = DOMpurify.sanitize(item.text);
    const createdDate = formatDate(new Date(item.createdAt));
    const styles = style ?? { minWidth: 300 };
    return (
        <Card
            key={item.id}
            style={styles}
            onClick={() => { onClick?.(item.id) }}>
            <Card.Meta
                title={`${item.personName} on ${createdDate}`}
                description={
                    <div dangerouslySetInnerHTML={{ __html: clean }} />
                }>
            </Card.Meta>
        </Card>
    )
}