class Falling {
  start() {
    this.setStep();
    $(window).resize(this.setStep.bind(this));
    this.setPiece();
    setInterval(this.fall.bind(this), 200);
  }

  get anim() {
    return $('.Anim');
  }

  get pieces() {
    return ['square', 'i', 'l', 't', 's'];
  }

  setStep() {
    if ($(window).width() < 1024) {
      this.step = 25;
    } else {
      this.step = 50;
    }
  }

  setPiece() {
    const next = this.pieces[Math.floor(Math.random()*this.pieces.length)];
    this.anim.attr('data-transform', -this.step * 3);
    this.anim.find('.Anim-piece').css('transform', `translateY(${-this.step * 3}px)`)
    this.anim.find('.Anim-piece').attr('data-name', next);

    const nextColumn = Math.random()*(this.anim.width() - this.anim.find('.Anim-piece').width());
    this.anim.find('.Anim-column').css('transform', `translateX(${nextColumn}px)`)
  }

  fall() {
    var currentTransform = parseInt(this.anim.attr('data-transform'));

    const nextTransform = currentTransform + this.step;
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
