import React from 'react'
import { useSteps } from 'mdx-deck'

export const Carousel = props => {
  const children = React.Children.toArray(props.children)
  const step = useSteps(children.length)
  const item = children[step - 1] || null;
  return <>{item}</>
}

export default Carousel