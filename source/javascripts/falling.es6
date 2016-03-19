class Falling {
  start() {
    this.setPiece();
    setInterval(this.fall.bind(this), 200);
  }

  get anim() {
    return $('.Anim');
  }

  get pieces() {
    return ['square', 'i', 'l', 't', 's'];
  }

  setPiece() {
    const next = this.pieces[Math.floor(Math.random()*this.pieces.length)];
    this.anim.attr('data-transform', -150);
    this.anim.find('.Anim-piece').css('transform', `translateY(-150px)`)
    this.anim.find('.Anim-piece').attr('data-name', next);

    const width = this.anim.width();
    const columnWidth = this.anim.find('.Anim-')
    const nextColumn = Math.random()*(this.anim.width() - this.anim.find('.Anim-piece').width());
    this.anim.css('margin-left', `${nextColumn}px`)
  }

  fall() {
    var currentTransform = parseInt(this.anim.attr('data-transform'));

    const nextTransform = currentTransform + 50;
    this.anim.attr('data-transform', nextTransform);

    this.anim.find('.Anim-piece').css('transform', `translateY(${nextTransform}px)`)

    this.checkColumnEnd(nextTransform);
  }

  checkColumnEnd(transform) {
    if (this.anim.height() < transform) {
      this.setPiece();
    }
  }
}
